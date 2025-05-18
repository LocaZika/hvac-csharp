namespace hvac_backend.users.customers.enums;

public enum EAccountType {
  Local,
  Google,
}
public static class EAccountTypeExtensions {
  /// <summary>
  /// Gets custom value reference enum value.
  /// </summary>
  /// <param name="accountType"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  public static string ToEnumString(this EAccountType accountType) => accountType switch {
    EAccountType.Local => "local",
    EAccountType.Google => "google",
    _ => throw new ArgumentOutOfRangeException($"{accountType} is not in EAccountType"),
  };
  public static string[] GetValidValues()
    => [.. Enum.GetValues<EAccountType>().Select(act => act.ToString())];
  public static string GetStringOfValidValues()
    => string.Join(", ", Enum.GetValues<EAccountType>().Select(act => act.ToString()));
}
