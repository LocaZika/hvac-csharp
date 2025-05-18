namespace hvac_backend.utilities;

public class EnumHelper {
  public static bool IsValidEnumValue<TEnum>(string value) where TEnum : Enum {
    if (string.IsNullOrWhiteSpace(value)) return false; ;

    // Lấy tất cả tên Enum gốc
    var enumNames = Enum.GetNames(typeof(TEnum));

    // Lấy tất cả chuỗi ánh xạ (nếu phương thức ToTradeTypeString tồn tại)
    var getStringValueFnNameMethod = typeof(TEnum).GetMethod("ToEnumString", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
    var mappedValues = getStringValueFnNameMethod != null
      ? typeof(TEnum)
          .GetEnumValues()
          .Cast<TEnum>()
          .Select(e => getStringValueFnNameMethod.Invoke(null, [e])?.ToString())
      : null;

    // So sánh với cả tên Enum gốc và chuỗi ánh xạ
    return enumNames.Any(e => string.Equals(e, value, StringComparison.OrdinalIgnoreCase)) ||
      (mappedValues?.Any(mv => string.Equals(mv, value, StringComparison.OrdinalIgnoreCase)) ?? false);
  }


}
