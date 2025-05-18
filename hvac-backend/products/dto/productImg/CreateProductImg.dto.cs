using FluentValidation;

namespace hvac_backend.products.dto.productImg;

public class CreateProductImgDto {
  public int ProductId { get; set; }
  public required string[] Imgs { get; set; }
}

public class CreateProductImgValidator : AbstractValidator<CreateProductImgDto> {
  public CreateProductImgValidator() {
    RuleFor(x => x.ProductId)
    .NotEmpty().WithMessage("Product id is required");
    RuleFor(x => x.Imgs)
    .NotEmpty().WithMessage("Product imgs is required")
    .Must(v => v is not null).WithMessage("Imgs must be a string array")
    .Must(v => v is not null && v.Length > 0).WithMessage("Imgs must have item")
    .ForEach(v => {
      v.NotEmpty().WithMessage("Item must be a valid string");
    });
  }
}