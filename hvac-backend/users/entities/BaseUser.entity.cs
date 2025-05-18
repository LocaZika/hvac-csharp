using hvac_backend.global.entities;

namespace hvac_backend.users.entities;

public class BaseUserEntity : BaseEntity {
  public required string Email { get; set; }
  public required string Password { get; set; }
  public required string Phone { get; set; }
  public required string Address { get; set; }
  public required string Role { get; set; }
}
