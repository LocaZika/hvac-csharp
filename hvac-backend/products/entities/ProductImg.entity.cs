using hvac_backend.global.entities;

namespace hvac_backend.products.entities;

public class ProductImg : BaseImgEntity {
  public required int ProductId { get; set; }
  public Product? Product { get; set; }
}

public class ProductDetailImg : BaseImgEntity {
  public required int ProductId { get; set; }
  public Product? Product { get; set; }
}