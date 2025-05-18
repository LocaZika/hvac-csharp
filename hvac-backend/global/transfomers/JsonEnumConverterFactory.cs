using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Concurrent;

namespace hvac_backend.global.transfomers;

public class JsonEnumConverterFactory : JsonConverterFactory {
  // Cache cho tất cả JsonConverter của Enum
  private static readonly ConcurrentDictionary<Type, JsonConverter> _converterCache = new();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert.IsEnum;
  }

  public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
    // Nếu đã có trong cache thì trả về luôn
    if (_converterCache.TryGetValue(typeToConvert, out var cachedConverter))
      return cachedConverter;

    // Tạo CachedEnumConverter cho Enum
    var converterType = typeof(CachedEnumConverter<>).MakeGenericType(typeToConvert);
    var converter = (JsonConverter?)Activator.CreateInstance(converterType) ?? throw new InvalidOperationException($"Could not create converter for type {typeToConvert}.");
    // Thêm vào cache
    _converterCache.TryAdd(typeToConvert, converter);
    return converter;
  }
}

// Lớp CachedEnumConverter như đã tối ưu trước đó
public class CachedEnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum {
  private static readonly ConcurrentDictionary<string, TEnum> _enumCache = new();

  static CachedEnumConverter() {
    // Khởi tạo cache cho tất cả tên Enum gốc
    foreach (var enumValue in Enum.GetValues<TEnum>()) {
      _enumCache.TryAdd(enumValue.ToString(), enumValue);
    }

    // Nếu có phương thức ToEnumString, thêm chuỗi ánh xạ vào cache
    var ToEnumStringMethod = typeof(TEnum).GetMethod("ToEnumString");
    if (ToEnumStringMethod != null) {
      foreach (var enumValue in Enum.GetValues<TEnum>()) {
        var mappedValue = ToEnumStringMethod.Invoke(enumValue, null) as string;
        if (!string.IsNullOrEmpty(mappedValue)) {
          _enumCache.TryAdd(mappedValue.ToLower(), enumValue);
        }
      }
    }
  }

  public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    var value = reader.GetString();
    if (string.IsNullOrWhiteSpace(value))
      throw new JsonException($"Value cannot be null or empty for {typeof(TEnum).Name}.");

    // Tra cứu từ cache
    if (_enumCache.TryGetValue(value.Trim(), out var enumValue) ||
        _enumCache.TryGetValue(value.Trim().ToLower(), out enumValue)) {
      return enumValue;
    }

    throw new JsonException($"Invalid value '{value}' for {typeof(TEnum).Name}. Allowed values: {string.Join(", ", _enumCache.Keys)}.");
  }

  public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options) {
    // Ghi lại chuỗi ánh xạ nếu có
    var ToEnumStringMethod = typeof(TEnum).GetMethod("ToEnumString");
    if (ToEnumStringMethod != null) {
      var mappedValue = ToEnumStringMethod.Invoke(value, null) as string;
      if (!string.IsNullOrEmpty(mappedValue)) {
        writer.WriteStringValue(mappedValue);
        return;
      }
    }

    // Ghi lại tên Enum gốc nếu không có chuỗi ánh xạ
    writer.WriteStringValue(value.ToString());
  }
}
