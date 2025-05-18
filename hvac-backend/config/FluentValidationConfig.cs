using FluentValidation;
using hvac_backend.products.dto;

namespace hvac_backend.config;

public class FluentValidationConfig {
  public static void Register(WebApplicationBuilder builder) {
    builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDto>();
    builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductDto>();
    builder.Services.AddValidatorsFromAssemblyContaining<ProductQueryParamsDto>();
  }
}
