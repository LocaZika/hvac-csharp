using System.Text.RegularExpressions;

namespace hvac_backend.utilities;

public class Interpolator {
  public static string EnvInterpolate(string input) {
    if (string.IsNullOrEmpty(input)) return input;
    string pattern = @"\$\{([A-Z0-9_]+)\}";
    return Regex.Replace(input, pattern, match => {
      var envVar = match.Groups[1].Value;
      var value = Environment.GetEnvironmentVariable(envVar);
      if (value == null) {
        Console.WriteLine($"Env variable '{value}' was not found");
        return match.Value;
      }
      return value;
    });
  }
}
