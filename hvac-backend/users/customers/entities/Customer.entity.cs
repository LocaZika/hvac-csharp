using hvac_backend.users.customers.enums;
using hvac_backend.users.entities;

namespace hvac_backend.users.customers.entities;

public class Customer : BaseUserEntity {
  public required EAccountType AccountType { get; set; }
  public required bool IsActive { get; set; }
  public required string CodeId { get; set; }
  public DateTimeOffset CodeExpire { get; set; }
}
