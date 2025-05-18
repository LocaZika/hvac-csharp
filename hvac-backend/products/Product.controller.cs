using FluentValidation;
using FluentValidation.Results;
using hvac_backend.global.response;
using hvac_backend.products.dto;
using hvac_backend.products.dto.productImg;
using Microsoft.AspNetCore.Mvc;

namespace hvac_backend.products;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(
  ProductService productService,
  IValidator<CreateProductDto> createProductValidator,
  IValidator<UpdateProductDto> updateProductValidator,
  IValidator<CreateProductImgDto> createProductImgValidator,
  IValidator<ProductQueryParamsDto> productQueryParamsValidator
) : ControllerBase {
  private readonly ProductService Service = productService;
  private readonly IValidator<CreateProductDto> CreateProductValidator = createProductValidator;
  private readonly IValidator<UpdateProductDto> UpdateProductValidator = updateProductValidator;
  private readonly IValidator<CreateProductImgDto> CreateProductImgValidator = createProductImgValidator;
  private readonly IValidator<ProductQueryParamsDto> ProductQueryParamsValidator = productQueryParamsValidator;

  [HttpGet]
  public async Task<IActionResult> FindAll([FromQuery] ProductQueryParamsDto productQueryParams) {
    try {
      ValidationResult validationResult = ProductQueryParamsValidator.Validate(productQueryParams);
      if (!validationResult.IsValid) {
        return BadRequest(ApiResponse.Failed(400, [.. validationResult.Errors.Select(e => e.ErrorMessage)]));
      }
      var products = await Service.FindAll(productQueryParams);
      if (products == null) {
        return BadRequest("Invalid query params");
      }
      if (products.Count == 0) {
        return NoContent();
      }
      return Ok(ApiResponse<List<ProductWithImgsDto>>.Success(products));
    }
    catch (Exception ex) {
      Console.WriteLine($"message: {ex.Message}\n stackTrace: {ex.StackTrace}");
      return Problem("Internal Server Error00");
    }
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> FindOne([FromRoute] string id) {
    try {
      if (int.TryParse(id, out int result)) {
        var product = await Service.FindOne(result);
        if (product == null) {
          return NotFound($"Product was not found with id: '{id}'");
        }
        return Ok(ApiResponse<ProductWithDetailImgsDto>.Success(product));
      }
      return BadRequest(ApiResponse.Failed(400, ["Invalid Id"]));
    }
    catch {
      return Problem("Internal Server Error");
    }
  }

  // [PrivateRoute(Role.Admin, Role.Employee, Role.Manager)]
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateProductDto productDto) {
    try {
      ValidationResult validationResult = CreateProductValidator.Validate(productDto);
      if (!validationResult.IsValid) {
        return BadRequest(ApiResponse.Failed(400, [.. validationResult.Errors.Select(e => e.ErrorMessage)]));
      }
      bool isExisted = await Service.Create(productDto);
      if (!isExisted) {
        return Conflict(ApiResponse.Failed(409, [$"Product is existed with Name '{productDto.Name}' and Brand '{productDto.Brand}'"]));
      }
      return Created("/api/products", new ApiResponse(201, ["Product was created successfully"]));
    }
    catch {
      return Problem("Internal Server Error");
    }
  }

  // [PrivateRoute(Role.Admin, Role.Employee, Role.Manager)]
  [HttpPatch("{id}")]
  public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateProductDto productDto) {
    try {
      if (int.TryParse(id, out int result)) {
        ValidationResult validationResult = UpdateProductValidator.Validate(productDto);
        if (!validationResult.IsValid) {
          return BadRequest(ApiResponse.Failed(400, [.. validationResult.Errors.Select(e => e.ErrorMessage)]));
        }
        bool isExisted = await Service.Update(result, productDto);
        if (!isExisted) {
          return NotFound($"Product was not found with id: '{id}'");
        }
        return Created("/api/products", new ApiResponse(200, [$"Product was update successfully with id: '{id}'"]));
      }
      return BadRequest(ApiResponse.Failed(400, ["Invalid Id"]));
    }
    catch {
      return Problem("Internal Server Error");
    }
  }

  // [PrivateRoute(Role.Admin, Role.Manager)]
  [HttpDelete("{id}")]
  public async Task<IActionResult> Remove([FromRoute] string id) {
    try {
      if (int.TryParse(id, out int result)) {
        bool isExisted = await Service.Remove(result);
        if (!isExisted) {
          return NotFound($"Product was not found with id: '{id}'");
        }
        return Ok(ApiResponse.Success(200, [$"Product was removed successfully with id: '{id}'"]));
      }
      return BadRequest(ApiResponse.Failed(400, ["Invalid product Id"]));
    }
    catch {
      return Problem("Internal Server Error");
    }
  }

  // [PrivateRoute(Role.Admin, Role.Employee, Role.Manager)]
  [HttpPost("img")]
  public async Task<IActionResult> CreateProductImg([FromBody] CreateProductImgDto productImgDto) {
    try {
      ValidationResult validationResult = CreateProductImgValidator.Validate(productImgDto);
      if (!validationResult.IsValid) {
        return BadRequest(ApiResponse.Failed(400, [.. validationResult.Errors.Select(e => e.ErrorMessage)]));
      }
      bool isExisted = await Service.CreateProductImg(productImgDto);
      if (!isExisted) {
        return BadRequest(ApiResponse.Failed(400, [$"Product is not found with id: {productImgDto.ProductId}"]));
      }
      else {
        return Created("/api/products/productimg", ApiResponse.Success(201, ["Product Imgs was created successfully"]));
      }
    }
    catch {
      return Problem("Internal Server Error");
    }
  }

  // [PrivateRoute(Role.Admin, Role.Employee, Role.Manager)]
  [HttpPost("detailimg")]
  public async Task<IActionResult> CreateProductDetailImg([FromBody] CreateProductImgDto productDetailImgDto) {
    try {
      ValidationResult validationResult = CreateProductImgValidator.Validate(productDetailImgDto);
      if (!validationResult.IsValid) {
        return BadRequest(ApiResponse.Failed(400, [.. validationResult.Errors.Select(e => e.ErrorMessage)]));
      }
      bool isExisted = await Service.CreateProductImg(productDetailImgDto);
      if (!isExisted) {
        return BadRequest(ApiResponse.Failed(400, [$"Product is not found with id: {productDetailImgDto.ProductId}"]));
      }
      else {
        return Created("/api/products/productimg", ApiResponse.Success(201, ["Product Imgs was created successfully"]));
      }
    }
    catch {
      return Problem("Internal Server Error");
    }
  }

}
