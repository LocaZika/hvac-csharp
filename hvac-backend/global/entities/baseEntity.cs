namespace hvac_backend.global.entities;

public abstract class BaseEntity {
  public int Id { get; set; }
  public DateTimeOffset Created_at { get; private set; }
  public DateTimeOffset Updated_at { get; set; }
}
