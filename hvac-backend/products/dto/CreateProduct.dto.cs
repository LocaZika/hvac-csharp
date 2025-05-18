using FluentValidation;
using hvac_backend.products.enums;

namespace hvac_backend.products.dto;

public class CreateProductDto {
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

public class CreateProductValidator : AbstractValidator<CreateProductDto> {
  public CreateProductValidator() {
    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("Product name is required")
      .MinimumLength(5).WithMessage("Product name must have at least 5 characters")
      .MaximumLength(50).WithMessage("Product name must be less than 50 characters");
    RuleFor(x => x.Brand)
      .NotEmpty().WithMessage("Product brand is required")
      .MinimumLength(4).WithMessage("Product brand must have at least 4 characters")
      .MaximumLength(15).WithMessage("Product brand must be less than 15 characters");
    RuleFor(x => x.Price)
      .NotEmpty().WithMessage("Product price is required")
      .Must(v => v.HasValue && HaveValidPrecision(v.Value)).WithMessage("Product price must have at most 10 digits in total, including 2 decimal places.")
      .GreaterThan(100).WithMessage("Product price must greater than 100");
    RuleFor(x => x.Transmission)
      .NotEmpty().WithMessage("Product transmission is required");
    RuleFor(x => x.TradeType)
      .NotEmpty().WithMessage("Product tradeType is required");
    RuleFor(x => x.FuelType)
      .NotEmpty().WithMessage("Product fuelType is required");
    RuleFor(x => x.Type)
      .NotEmpty().WithMessage("Product type is required")
      .MinimumLength(3).WithMessage("Product type must have at least 3 characters")
      .MaximumLength(10).WithMessage("Product type must be less than 10 characters");
    RuleFor(x => x.Hp)
      .NotEmpty().WithMessage("Product hp is required")
      .InclusiveBetween((short)20, (short)2000).WithMessage("Product hp must be between 20 and 2000");
    RuleFor(x => x.Model)
      .NotEmpty().WithMessage("Product model is required")
      .InclusiveBetween((short)1965, (short)2100).WithMessage("Product model must be between 1965 and 2100");
    RuleFor(x => x.Mileage)
      .NotEmpty().WithMessage("Product mileage is required")
      .GreaterThanOrEqualTo((short)0).WithMessage("Product mileage must be positive number");
    RuleFor(x => x.Vin)
      .NotEmpty().WithMessage("Product vin is required")
      .Must(v => !string.IsNullOrWhiteSpace(v) && v.Length == 12).WithMessage("Product VIN must be 12 characters");
    RuleFor(x => x.Stock)
      .NotEmpty().WithMessage("Product stock is required")
      .Must(v => !string.IsNullOrWhiteSpace(v) && v.Length == 12).WithMessage("Product STOCK must be 12 characters");
  }
  private static bool HaveValidPrecision(decimal price) {
    var absPrice = Math.Abs(price);
    var part = absPrice.ToString(System.Globalization.CultureInfo.InvariantCulture).Split(".");
    var intDigits = part[0].TrimStart('0').Length;
    var decimalDigits = part.Length > 1 ? part[1].Length : 0;
    var totalDigits = intDigits + decimalDigits;
    return totalDigits <= 10 && decimalDigits <= 2;
  }

}