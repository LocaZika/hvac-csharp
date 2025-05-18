namespace hvac_backend.products.enums;

public enum ETradeType {
  Sale,
  Rent,
}
public static class ETradeTypeExtensions {
  /// <summary>
  /// Gets custom value reference enum value.
  /// </summary>
  /// <param name="accountType"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  public static string ToEnumString(this ETradeType ETradeType) => ETradeType switch {
    ETradeType.Sale => "sale",
    ETradeType.Rent => "rent",
    _ => throw new ArgumentOutOfRangeException($"{ETradeType} is not in ETradeType"),
  };
  public static string[] GetValidValues()
    => [.. Enum.GetValues<ETradeType>().Select(tt => tt.ToEnumString())];
  public static string GetStringOfValidValues()
    => string.Join(", ", Enum.GetValues<ETradeType>().Select(tt => tt.ToEnumString()));
}