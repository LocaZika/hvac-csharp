using FluentValidation;

namespace hvac_backend.users.customers.dtos;

public class UpdateCustomerDto {
  public string? Email { get; set; }
  public string? Password { get; set; }
  public string? Phone { get; set; }
  public string? Address { get; set; }
}

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto> {
  public UpdateCustomerValidator() {
    RuleFor(x => x.Email)
      .Matches("^[a-zA-Z0-9._]+@[a-z]+\\.[a-z]{2,5}$").WithMessage("{PropertyValue} is a invalid email")
      .When(x => !string.IsNullOrWhiteSpace(x.Email));
    RuleFor(x => x.Password)
      .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$").WithMessage("{PropertyName} must be at least 8 characters with uppercase, lowercase and special character")
      .When(x => !string.IsNullOrWhiteSpace(x.Email));
    RuleFor(x => x.Phone)
      .Matches("^(0[1-9]{9}|\\+84[1-9]{9})$").WithMessage("{PropertyName} is not a valid phone number. Eg: 0123456789, +84123456789")
      .When(x => !string.IsNullOrWhiteSpace(x.Email));
  }
}
