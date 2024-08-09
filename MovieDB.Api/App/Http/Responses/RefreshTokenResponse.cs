namespace MovieDB.Api.App.Http.Responses;

public class RefreshTokenResponse
{
    public required string JwtToken { get; set; }
    public required string RefreshToken { get; set; }
}
