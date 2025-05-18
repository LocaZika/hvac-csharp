namespace hvac_backend.products.enums;

public enum ESortBy {
  Asc,
  Desc,
}
public static class ESortByExtensions {
  /// <summary>
  /// Gets custom value reference enum value.
  /// </summary>
  /// <param name="accountType"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  public static string ToEnumString(this ESortBy eSortBy) => eSortBy switch {
    ESortBy.Asc => "asc",
    ESortBy.Desc => "desc",
    _ => throw new ArgumentOutOfRangeException($"{eSortBy} is not in ESortBy"),
  };
  public static string[] GetValidValues()
    => [.. Enum.GetValues<ESortBy>().Select(sb => sb.ToEnumString())];
  public static string GetStringOfValidValues()
    => string.Join(", ", Enum.GetValues<ESortBy>().Select(sb => sb.ToEnumString()));
}
