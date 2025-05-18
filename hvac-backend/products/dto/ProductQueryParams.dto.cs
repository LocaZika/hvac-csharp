using FluentValidation;
using hvac_backend.products.enums;

namespace hvac_backend.products.dto;

public class ProductQueryParamsDto {
  public short? Page { get; set; } = 1;
  public string? SortBy { get; set; }
  public string? Q { get; set; }
  public string? Brand { get; set; }
  public string? Type { get; set; }
  public string? Transmission { get; set; }
  public short? Model { get; set; }
  public decimal? LastPrice { get; set; }
}

public class ProductQueryParamsValidator : AbstractValidator<ProductQueryParamsDto> {
  public ProductQueryParamsValidator() {
    RuleFor(x => x.Page)
      .GreaterThanOrEqualTo((short)1)
      .WithMessage("Page must be a number and greater than or equal to 1")
      .When(x => !string.IsNullOrWhiteSpace(x.Page.ToString()));
    RuleFor(x => x.Model)
      .Must(v => v != null && v >= 1995)
      .WithMessage("Product model must be a number and greater than or equal to 1995")
      .When(x => !string.IsNullOrWhiteSpace(x.Model.ToString()));
    RuleFor(x => x.Brand)
      .Matches("^[a-zA-Z\\s]+$")
      .WithMessage("Product brand must not have digits and special characters")
      .When(x => !string.IsNullOrWhiteSpace(x.Brand));
    RuleFor(x => x.SortBy)
      .Must(IsValidSortBy!)
      .WithMessage(x => $"{x.SortBy} must be in '{ESortByExtensions.GetStringOfValidValues()}'")
      .When(x => !string.IsNullOrWhiteSpace(x.SortBy));
    RuleFor(x => x.Q)
      .Matches("^[a-zA-Z0-9\\s]+$")
      .WithMessage("Search keyword must not have special characters")
      .When(x => !string.IsNullOrWhiteSpace(x.Q));
    RuleFor(x => x.Type)
      .Matches("^[a-zA-Z\\s]+$")
      .WithMessage("Product Type must not have special characters and digits")
      .When(x => !string.IsNullOrWhiteSpace(x.Type));
    RuleFor(x => x.Transmission)
      .Must(IsValidTransmission!)
      .WithMessage(x => $"{x.Transmission} must be one of '{ETransmissionExtensions.GetStringOfValidValues()}'")
      .When(x => !string.IsNullOrWhiteSpace(x.Transmission));
    RuleFor(x => x.LastPrice)
      .Must(v => v.HasValue && IsValidLastPrice(v.Value))
      .WithMessage("Last price must be a numeric(10, 2)")
      .When(x => x.LastPrice > 0);
  }
  private static bool IsValidLastPrice(decimal v) {
    var absPrice = Math.Abs(v);
    var part = absPrice.ToString(System.Globalization.CultureInfo.InvariantCulture).Split(".");
    var intDigits = part[0].TrimStart('0').Length;
    var decimalDigits = part.Length > 1 ? part[1].Length : 0;
    var totalDigits = intDigits + decimalDigits;
    return totalDigits <= 10 && decimalDigits <= 2;
  }
  private static bool IsValidSortBy(string value)
    => Enum.GetValues<ESortBy>().Select(e => e.ToEnumString()).Contains(value);
  private static bool IsValidTransmission(string value)
    => Enum.GetValues<ETransmission>().Select(e => e.ToEnumString()).Contains(value);
}