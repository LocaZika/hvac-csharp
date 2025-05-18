namespace hvac_backend.products.enums;

public enum ETransmission {
  Auto,
  Manual,
  Mixed,
}
public static class ETransmissionExtensions {
  /// <summary>
  /// Gets custom value reference enum value.
  /// </summary>
  /// <param name="accountType"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  public static string ToEnumString(this ETransmission ETransmission) => ETransmission switch {
    ETransmission.Auto => "auto",
    ETransmission.Manual => "manual",
    ETransmission.Mixed => "mixed",
    _ => throw new ArgumentOutOfRangeException($"{ETransmission} is not in ETransmission"),
  };
  public static string[] GetValidValues()
    => [.. Enum.GetValues<ETransmission>().Select(t => t.ToEnumString())];
  public static string GetStringOfValidValues()
    => string.Join(", ", Enum.GetValues<ETransmission>().Select(t => t.ToEnumString()));
}
