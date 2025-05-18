namespace hvac_backend.utilities;

public class Uuid {
  public static string Generate() => UUID.New().ToString();
  public static bool IsEqual(string value1, string value2) => value1 == value2;
}
