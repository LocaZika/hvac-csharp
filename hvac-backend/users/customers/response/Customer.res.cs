namespace hvac_backend.users.customers.response;

public class CustomerResponse {
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public string? AccountType { get; set; }
  public string? Address { get; set; }
  public bool IsActive { get; set; }
}
