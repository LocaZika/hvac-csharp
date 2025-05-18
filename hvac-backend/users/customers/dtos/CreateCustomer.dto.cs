using FluentValidation;

namespace hvac_backend.users.customers.dtos;

public class CreateCustomerDto {
  public string? Email { get; set; }
  public string? Password { get; set; }
  public string? Phone { get; set; }
  public string? Address { get; set; }
}

public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto> {
  public CreateCustomerValidator() {
    RuleFor(x => x.Email)
      .NotEmpty().WithMessage("Customer's email is required")
      .Matches("^[a-zA-Z0-9._]+@[a-z]+\\.[a-z]{2,5}$").WithMessage("{PropertyValue} is a invalid email");
    RuleFor(x => x.Password)
      .NotEmpty().WithMessage("Customer's password is required")
      .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$").WithMessage("{PropertyName} must be at least 8 characters with uppercase, lowercase and special character");
    RuleFor(x => x.Phone)
      .NotEmpty().WithMessage("Customer's phone is required")
      .Matches("^(0[1-9]{9}|\\+84[1-9]{9})$").WithMessage("{PropertyName} is not a valid phone number. Eg: 0123456789, +84123456789");
    RuleFor(x => x.Address)
      .NotEmpty().WithMessage("Customer's address is required");
  }
}
