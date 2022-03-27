using System.Security.Claims;
using MovieDB.Api.Entities;
using MovieDB.Api.Helpers;

namespace MovieDB.Api.Middleware;

public class UserMiddleware
{
    private readonly RequestDelegate _next;

    public UserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        var id = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        var accountId = int.Parse(id);

        context.Items[nameof(Account)] = await db.Accounts.FindAsync(accountId);

        await _next(context);
    }
}
