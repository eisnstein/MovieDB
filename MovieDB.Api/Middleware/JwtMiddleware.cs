using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using MovieDB.Api.Data;
using MovieDB.Api.Helpers;

namespace MovieDB.Api.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppSettings _settings;

        public JwtMiddleware(
            RequestDelegate next,
            IAppSettings settings)
        {
            _next = next;
            _settings = settings;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext db)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader is not null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Split(" ").Last();
                await AttachTokenToContext(context, token, db);
            }

            await _next(context);
        }

        private async Task AttachTokenToContext(HttpContext context, string token, AppDbContext db)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_settings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken) validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(c => c.Type == "id").Value);

                context.Items["Account"] = await db.Accounts.FindAsync(accountId);
            }
            catch
            {
            }
        }
    }
}
