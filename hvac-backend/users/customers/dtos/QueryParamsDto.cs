using FluentValidation;

namespace hvac_backend.users.customers.dtos;

public class QueryParamsCustomerDto {
  public short? Page { get; set; } = 1;
  public string? Q { get; set; }
}
public class QueryParamsCustomerValidator : AbstractValidator<QueryParamsCustomerDto> {
  public QueryParamsCustomerValidator() {
    RuleFor(x => x.Q)
      .Matches("^[a-zA-Z0-9\\s]+$")
      .WithMessage("Search words is invalid")
      .When(x => !string.IsNullOrWhiteSpace(x.Q));
  }
}
