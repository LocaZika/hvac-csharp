using hvac_backend.global.entities;
using hvac_backend.products.enums;

namespace hvac_backend.products.entities;

public class Product : BaseEntity {
  public required string Name { get; set; }
  public required string Brand { get; set; }
  public required decimal Price { get; set; }
  public required ETransmission Transmission { get; set; }
  public required ETradeType TradeType { get; set; }
  public required EFuelType FuelType { get; set; }
  public required string Type { get; set; }
  public required short Hp { get; set; }
  public required short Model { get; set; }
  public required short Mileage { get; set; }
  public required string Vin { get; set; }
  public required string Stock { get; set; }
  public ICollection<ProductImg> Imgs { get; set; } = [];
  public ICollection<ProductDetailImg> DetailImgs { get; set; } = [];
}
