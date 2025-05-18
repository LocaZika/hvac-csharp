using hvac_backend.products.enums;

namespace hvac_backend.products.dto;

public class ProductDto {
  public string? Name { get; set; }
  public decimal? Price { get; set; }
  public ETransmission? Transmission { get; set; }
  public ETradeType? TradeType { get; set; }
  public short? Hp { get; set; }
  public short? Model { get; set; }
  public short? Mileage { get; set; }
}

public class ProductImgDto {
  public string Path { get; set; } = default!;
}

public class ProductWithImgsDto : ProductDto {
  public int Id { get; set; }
  public required List<ProductImgDto> Imgs { get; set; }

}
public class ProductWithDetailImgsDto : ProductDto {
  public required List<ProductImgDto> Imgs { get; set; }
  public required List<ProductImgDto> DetailImgs { get; set; }

}