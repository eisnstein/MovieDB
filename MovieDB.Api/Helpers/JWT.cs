using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Helpers;

public static class JWT
{
    public static string BuildToken(Account account, string secret)
    {
        var identity = new ClaimsIdentity(new []{ new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()) });
        var bytes = Encoding.UTF8.GetBytes("ETb9GALyuyhqcDCZBxz48vmEUKQEuqGd");
        var key = new SymmetricSecurityKey(bytes);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddDays(10),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
            Subject = identity
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}
