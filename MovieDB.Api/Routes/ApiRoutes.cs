using MovieDB.Api.App.Endpoints;

namespace MovieDB.Api.Routes;

public static class ApiRoutes
{
    public static void Definitions(WebApplication app)
    {
        // Account Endpoints
        app.MapPost("/api/accounts/authenticate", AccountsEndpoint.Authenticate);
        app.MapPost("/api/accounts/register", AccountsEndpoint.Register);
        app.MapPost("/api/accounts/verify-email", AccountsEndpoint.VerifyEmail);
        app.MapPost("/api/accounts/forgot-password", AccountsEndpoint.ForgotPassword);
        app.MapPost("/api/accounts/validate-reset-token", AccountsEndpoint.ValidateResetToken);
        app.MapPost("/api/accounts/reset-password", AccountsEndpoint.ResetPassword);
        app.MapPost("/api/accounts/refresh-token", AccountsEndpoint.RefreshToken);

        app.MapPost("/api/accounts/revoke-token", AccountsEndpoint.RevokeToken).RequireAuthorization();
        app.MapGet("/api/accounts/{id:int}", AccountsEndpoint.GetById).RequireAuthorization();
        app.MapPut("/api/accounts/{id:int}", AccountsEndpoint.Update).RequireAuthorization();

        // Concert Endpoints
        app.MapGet("/api/concerts", ConcertsEndpoints.GetAll).RequireAuthorization();
        app.MapGet("/api/concerts/{id:int}", ConcertsEndpoints.GetById).RequireAuthorization();
        app.MapPost("/api/concerts", ConcertsEndpoints.Create).RequireAuthorization();
        app.MapPut("/api/concerts/{id:int}", ConcertsEndpoints.Update).RequireAuthorization();
        app.MapDelete("/api/concerts/{id:int}", ConcertsEndpoints.Delete).RequireAuthorization();

        // Movie Endpoints
        app.MapGet("/api/movies", MoviesEndpoints.GetAll).RequireAuthorization();
        app.MapGet("/api/movies/{id:int}", MoviesEndpoints.GetById).RequireAuthorization();
        app.MapPost("/api/movies", MoviesEndpoints.Create).RequireAuthorization();
        app.MapPut("/api/movies/{id:int}", MoviesEndpoints.Update).RequireAuthorization();
        app.MapDelete("/api/movies/{id:int}", MoviesEndpoints.Delete).RequireAuthorization();

        // Theater Endpoints
        app.MapGet("/api/theaters", TheatersEndpoints.GetAll).RequireAuthorization();
        app.MapGet("/api/theaters/{id:int}", TheatersEndpoints.GetById).RequireAuthorization();
        app.MapPost("/api/theaters", TheatersEndpoints.Create).RequireAuthorization();
        app.MapPut("/api/theaters/{id:int}", TheatersEndpoints.Update).RequireAuthorization();
        app.MapDelete("/api/theaters/{id:int}", TheatersEndpoints.Delete).RequireAuthorization();
    }
}
