namespace hvac_backend.utilities;

public class Time {
  private static readonly DateTimeOffset currentDateTimeOffset = DateTimeOffset.Now;
  public static DateTimeOffset AddMinutes(double minutes) => currentDateTimeOffset.AddMinutes(minutes);
  public static DateTimeOffset AddHours(double hours) => currentDateTimeOffset.AddHours(hours);
  public static DateTimeOffset AddDays(double days) => currentDateTimeOffset.AddDays(days);
  public static DateTimeOffset AddMonths(short months) => currentDateTimeOffset.AddMonths(months);
  public static bool IsBefore(DateTimeOffset time1, DateTimeOffset time2) {
    var result = DateTimeOffset.Compare(time1, time2);
    if (result < 0) return true;
    return false;
  }
  public static bool IsBeforeOrEquals(DateTimeOffset time1, DateTimeOffset time2) {
    var result = DateTimeOffset.Compare(time1, time2);
    if (result <= 0) return true;
    return false;
  }
  public static bool IsAfter(DateTimeOffset time1, DateTimeOffset time2) {
    var result = DateTimeOffset.Compare(time1, time2);
    if (result > 0) return true;
    return false;
  }
  public static bool IsAfterOrEquals(DateTimeOffset time1, DateTimeOffset time2) {
    var result = DateTimeOffset.Compare(time1, time2);
    if (result >= 0) return true;
    return false;
  }
}
