namespace hvac_backend.utilities;

public class Bcrypt {
  public static string Encode(string value) => BCrypt.Net.BCrypt.HashPassword(value);
  public static bool Compare(string value, string encodedValue) => BCrypt.Net.BCrypt.Verify(value, encodedValue);

}
