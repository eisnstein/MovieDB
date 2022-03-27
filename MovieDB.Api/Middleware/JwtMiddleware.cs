using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieDB.Api.Entities;
using MovieDB.Api.Helpers;

namespace MovieDB.Api.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _settings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> settings)
    {
        _next = next;
        _settings = settings.Value;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader is not null && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Split(" ").Last();
            await AttachAccountToContext(context, token, db);
        }

        await _next(context);
    }

    private async Task AttachAccountToContext(HttpContext context, string token, AppDbContext db)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ETb9GALyuyhqcDCZBxz48vmEUKQEuqGd");
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            var accountId = int.Parse(jwtToken.Subject);

            context.Items[nameof(Account)] = await db.Accounts.FindAsync(accountId);
        }
        catch
        {
            // ignored
        }
    }
}
