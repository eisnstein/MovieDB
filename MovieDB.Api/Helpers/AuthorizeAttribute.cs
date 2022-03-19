using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Helpers;

public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<Role> _roles;

    public AuthorizeAttribute(params Role[]? roles)
    {
        _roles = roles ?? Array.Empty<Role>();
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var account = context.HttpContext.Items.ContainsKey(nameof(Account)) && context.HttpContext.Items[nameof(Account)] is not null
            ? (Account) context.HttpContext.Items[nameof(Account)]!
            : null;

        if (account is null || (_roles.Any() && !_roles.Contains(account.Role)))
        {
            context.Result = new JsonResult(new { message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}
