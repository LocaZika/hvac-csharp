using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace hvac_backend.global.response;

public class CustomProblemDetails : ProblemDetails {
  [JsonPropertyName("statusCode")]
  public new int? Status { get; set; }

}
public class CustomValidationProblemDetails : ValidationProblemDetails {
  [JsonPropertyName("statusCode")]
  public new int? Status { get; set; }

  public CustomValidationProblemDetails() : base() { }

  public CustomValidationProblemDetails(ModelStateDictionary modelState) : base(modelState) { }
}

public class CustomProblemDetailsFactory(IOptions<ApiBehaviorOptions> options) : ProblemDetailsFactory {
  private readonly ApiBehaviorOptions Options = options.Value;
  public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null) {
    var problemDetails = new CustomProblemDetails() {
      Status = statusCode ?? StatusCodes.Status400BadRequest,
      Title = title ?? "Invalid Json input",
      Type = type ?? $"https://httpstatuses.com/{statusCode}",
      Detail = detail ?? "Internal Server Error.",
      Instance = instance ?? httpContext.Request.Path,
    };
    return problemDetails;
  }
  public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null) {
    var validationProblemDetails = new CustomValidationProblemDetails() {
      Status = statusCode ?? StatusCodes.Status400BadRequest,
      Title = title ?? "Invalid JSON Input",
      Type = type ?? "https://httpstatuses.com/400",
      Detail = detail ?? "Your request contains unexpected fields.",
      Instance = instance ?? httpContext?.Request?.Path,
    };
    return validationProblemDetails;
  }
}
