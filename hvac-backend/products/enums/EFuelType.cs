namespace hvac_backend.products.enums;

public enum EFuelType {
  Gasoline,
  Electric,
  Hybrid,
}
public static class EFuelTypeExtensions {
  /// <summary>
  /// Gets custom value reference enum value.
  /// </summary>
  /// <param name="accountType"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  public static string ToEnumString(this EFuelType eFuelType) => eFuelType switch {
    EFuelType.Gasoline => "gasoline",
    EFuelType.Electric => "electric",
    EFuelType.Hybrid => "hybrid",
    _ => throw new ArgumentOutOfRangeException($"{eFuelType} is not in EFuelType"),
  };
  public static string[] GetValidValues()
    => [.. Enum.GetValues<EFuelType>().Select(ft => ft.ToEnumString())];
  public static string GetStringOfValidValues()
    => string.Join(", ", Enum.GetValues<EFuelType>().Select(ft => ft.ToEnumString()));
}