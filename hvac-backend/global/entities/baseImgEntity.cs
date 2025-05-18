namespace hvac_backend.global.entities;

public class BaseImgEntity {
  public int Id { get; set; }
  public required string Path { get; set; }
  public string? Alt { get; set; }
}
