using System.Security.Claims;
using hvac_backend.global.types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace hvac_backend.global.attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class PrivateRouteAttribute : Attribute, IAuthorizationFilter {
  private readonly Role[] Roles;
  public PrivateRouteAttribute(params Role[] roles) {
    Roles = roles;
  }
  public void OnAuthorization(AuthorizationFilterContext ctx) {
    var user = ctx.HttpContext.User;
    if (!user.Identity?.IsAuthenticated ?? true) {
      ctx.Result = new UnauthorizedResult();
      return;
    }
    var userRole = user.FindFirst(ClaimTypes.Role)?.Value;
    if (userRole == null || !Roles.Select(r => r.ToRoleString()).Contains(userRole, StringComparer.OrdinalIgnoreCase)) {
      ctx.Result = new ForbidResult();
    }
  }
}
