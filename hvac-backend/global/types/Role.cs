namespace hvac_backend.global.types;

public enum Role {
  Admin,
  Customer,
  Employee,
  CEO,
  Manager,
}
public static class RoleExtensions {
  public static string ToRoleString(this Role role) => role switch {
    Role.Admin => "admin",
    Role.Customer => "customer",
    Role.Employee => "employee",
    Role.CEO => "ceo",
    Role.Manager => "manager",
    _ => throw new ArgumentOutOfRangeException($"{role} is not in Role enum.")
  };
}