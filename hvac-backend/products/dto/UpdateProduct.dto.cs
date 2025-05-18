using FluentValidation;
using hvac_backend.products.enums;

namespace hvac_backend.products.dto;

public class UpdateProductDto {
  public string? Name { get; set; }
  public string? Brand { get; set; }
  public decimal? Price { get; set; }
  public ETransmission? Transmission { get; set; }
  public ETradeType? TradeType { get; set; }
  public EFuelType? FuelType { get; set; }
  public string? Type { get; set; }
  public short? Hp { get; set; }
  public short? Model { get; set; }
  public short? Mileage { get; set; }
  public string? Vin { get; set; }
  public string? Stock { get; set; }
}

public class UpdateProductValidator : AbstractValidator<UpdateProductDto> {
  public UpdateProductValidator() {
    RuleFor(x => x.Name)
      .Length(5, 50).WithMessage("Product name must be between 5 and 50 characters")
      .When(x => !string.IsNullOrWhiteSpace(x.Name));
    RuleFor(x => x.Brand)
      .Length(4, 15).WithMessage("Product brand must be between 5 and 50 characters")
      .When(x => !string.IsNullOrWhiteSpace(x.Brand));
    RuleFor(x => x.Price)
      .GreaterThan(100).WithMessage("Product price must be greater than 100")
      .Must(HaveValidPrecision).WithMessage("Product price must have at most 10 digits in total, including 2 decimal places.")
      .When(x => x.Price.HasValue);
    RuleFor(x => x.Type)
      .Length(4, 15).WithMessage("Product type must be between 5 and 50 characters")
      .When(x => !string.IsNullOrWhiteSpace(x.Type));
    RuleFor(x => x.Hp)
      .InclusiveBetween((short)20, (short)2000).WithMessage("Product hp must be between 20 and 2000");
    RuleFor(x => x.Model)
      .InclusiveBetween((short)1965, (short)2100).WithMessage("Product model must be between 1965 and 2100");
    RuleFor(x => x.Mileage)
      .GreaterThanOrEqualTo((short)0).WithMessage("Product mileage must be positive number");
    RuleFor(x => x.Vin)
      .Must(x => x != null && x.Length == 12).WithMessage("Product VIN must be 12 characters")
      .When(x => !string.IsNullOrWhiteSpace(x.Vin));
    RuleFor(x => x.Stock)
      .Must(x => x != null && x.Length == 12).WithMessage("Product STOCK must be 12 characters")
      .When(x => !string.IsNullOrWhiteSpace(x.Stock));
  }
  private bool HaveValidPrecision(decimal? price) {
    if (!price.HasValue) {
      return true;
    }
    else {
      var absPrice = Math.Abs((decimal)price);
      var part = absPrice.ToString(System.Globalization.CultureInfo.InvariantCulture).Split(".");
      var intDigits = part[0].TrimStart('0').Length;
      var decimalDigits = part.Length > 1 ? part[1].Length : 0;
      var totalDigits = intDigits + decimalDigits;
      return totalDigits <= 10 && decimalDigits <= 2;
    }
  }
}