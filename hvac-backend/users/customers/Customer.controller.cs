using FluentValidation;
using FluentValidation.Results;
using hvac_backend.global.attributes;
using hvac_backend.global.response;
using hvac_backend.global.types;
using hvac_backend.users.customers.dtos;
using hvac_backend.users.customers.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hvac_backend.users.customers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomersController(
  CustomerService customerService,
  IValidator<CreateCustomerDto> createCustomerValidator,
  IValidator<UpdateCustomerDto> updateCustomerValidator
) : ControllerBase {
  private readonly CustomerService customerService = customerService;
  private readonly IValidator<CreateCustomerDto> CreateCustomerValidator = createCustomerValidator;
  private readonly IValidator<UpdateCustomerDto> UpdateCustomerValidator = updateCustomerValidator;

  [PrivateRoute(Role.Admin, Role.Employee)]
  [HttpGet]
  public async Task<IActionResult> FindAll([FromQuery] QueryParamsCustomerDto queryParams) {
    try {
      var customers = await customerService.FindAll(queryParams);
      if (customers == null) {
        return BadRequest("Invalid query params");
      }
      return Ok(ApiResponse<List<CustomerResponse>>.Success(customers));
    }
    catch (Exception ex) {
      return Problem("Internal Server Error", ex.Message);
    }
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> FindOne(string id) {
    try {
      if (!int.TryParse(id, out int validId)) {
        return BadRequest("Customer's id must be a integer number");
      }
      var customer = await customerService.FindOne(validId);
      if (customer == null) {
        return NotFound($"Customer was not found with id: {id}");
      }
      return Ok(ApiResponse<CustomerResponse>.Success(customer));
    }
    catch (Exception ex) {
      return Problem("Internal Server Error", ex.Message);
    }
  }

  [PrivateRoute(Role.Admin, Role.Employee)]
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateCustomerDto customerDto) {
    try {
      ValidationResult validationResult = CreateCustomerValidator.Validate(customerDto);
      if (!validationResult.IsValid) {
        return BadRequest(ApiResponse.Failed(400, [.. validationResult.Errors.Select(e => e.ErrorMessage)]));
      }
      var isExisted = await customerService.Create(customerDto);
      if (isExisted) {
        return Conflict("Email or Phone was existed");
      }
      return CreatedAtAction(nameof(Create), new ApiResponse(201, ["New customer was created successfully"]));
    }
    catch {
      return Problem("Internal Server Error");
    }
  }

  [PrivateRoute(Role.Admin, Role.Employee)]
  [HttpPatch("{id}")]
  public async Task<IActionResult> Update(string id, [FromBody] UpdateCustomerDto customerDto) {
    try {
      if (!int.TryParse(id, out int validId)) return BadRequest("Customer's id must a integer number");
      ValidationResult validationResult = UpdateCustomerValidator.Validate(customerDto);
      if (!validationResult.IsValid) {
        return BadRequest(ApiResponse.Failed(400, [.. validationResult.Errors.Select(e => e.ErrorMessage)]));
      }
      var isExisted = await customerService.Update(validId, customerDto);
      if (!isExisted) return NotFound($"Customer was not found with id: {id}");
      return NoContent();
    }
    catch {
      return Problem("Internal Server Error");
    }
  }

  [PrivateRoute(Role.Admin, Role.Employee)]
  [HttpDelete("{id}")]
  public async Task<IActionResult> Remove(string id) {
    try {
      if (!int.TryParse(id, out int validId)) return BadRequest($"Customer's id must be integer number");
      var isExisted = await customerService.Remove(validId);
      if (isExisted) return NotFound($"Customer was not found with id: {id}");
      return Ok(ApiResponse.Success(200, ["Customer was removed successfully"]));
    }
    catch {
      return Problem("Internal Server Error");
    }
  }
}
