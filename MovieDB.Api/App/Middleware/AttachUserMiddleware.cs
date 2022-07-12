using System.Security.Claims;
using MovieDB.Api.App.Helpers;
using MovieDB.Api.App.Models;

namespace MovieDB.Api.App.Middleware;

public class AttachUserMiddleware
{
    private readonly RequestDelegate _next;

    public AttachUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        var id = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(id, out var accountId))
        {
            context.Items[nameof(Account)] = await db.Accounts.FindAsync(accountId);
        }

        await _next(context);
    }
}
