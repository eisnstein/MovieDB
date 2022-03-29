using MovieDB.Api.Endpoints;

namespace MovieDB.Api.Routes;

public static class Routes
{
    private static string prefix = "/api";

    public static void Configure(WebApplication app)
    {
        app.MapPost("/accounts/authenticate", AccountsEndpoint.Authenticate);
        app.MapPost("/accounts/register", AccountsEndpoint.Register);
        app.MapPost("/accounts/verify-email", AccountsEndpoint.VerifyEmail);
        app.MapPost("/accounts/forgot-password", AccountsEndpoint.ForgotPassword);
        app.MapPost("/accounts/validate-reset-token", AccountsEndpoint.ValidateResetToken);
        app.MapPost("/accounts/reset-password", AccountsEndpoint.ResetPassword);
        app.MapPost("/accounts/refresh-token", AccountsEndpoint.RefreshToken);

        app.MapPost("/accounts/revoke-token", AccountsEndpoint.RevokeToken).RequireAuthorization();
        app.MapGet("/accounts/{id:int}", AccountsEndpoint.GetById).RequireAuthorization();
        app.MapPut("/accounts/{id:int}", AccountsEndpoint.Update).RequireAuthorization();
    }
}
