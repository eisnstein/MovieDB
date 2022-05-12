using MovieDB.Api.App.Endpoints;

namespace MovieDB.Api.Routes;

public static class ApiRoutes
{
    public static void Definitions(WebApplication app)
    {
        var apiGroup = app.MapGroup("/api");
        {
            // Account Endpoints
            var accountsGroupPublic = apiGroup.MapGroup("/accounts");
            {
                accountsGroupPublic.MapPost("/authenticate", AccountsEndpoint.Authenticate);
                accountsGroupPublic.MapPost("/register", AccountsEndpoint.Register);
                accountsGroupPublic.MapPost("/verify-email", AccountsEndpoint.VerifyEmail);
                accountsGroupPublic.MapPost("/forgot-password", AccountsEndpoint.ForgotPassword);
                accountsGroupPublic.MapPost("/validate-reset-token", AccountsEndpoint.ValidateResetToken);
                accountsGroupPublic.MapPost("/reset-password", AccountsEndpoint.ResetPassword);
                accountsGroupPublic.MapPost("/refresh-token", AccountsEndpoint.RefreshToken);
            }

            var accountsGroupPrivate = apiGroup.MapGroup("/accounts").RequireAuthorization();
            {
                accountsGroupPrivate.MapPost("/revoke-token", AccountsEndpoint.RevokeToken);
                accountsGroupPrivate.MapGet("/{id:int}", AccountsEndpoint.GetById);
                accountsGroupPrivate.MapPut("/{id:int}", AccountsEndpoint.Update);
            }

            // Concert Endpoints
            var concertsGroup = apiGroup.MapGroup("/concerts").RequireAuthorization();
            {
                concertsGroup.MapGet("/", ConcertsEndpoints.GetAll);
                concertsGroup.MapGet("/{id:int}", ConcertsEndpoints.GetById);
                concertsGroup.MapPost("/", ConcertsEndpoints.Create);
                concertsGroup.MapPut("/{id:int}", ConcertsEndpoints.Update);
                concertsGroup.MapDelete("/{id:int}", ConcertsEndpoints.Delete);
            }

            // Movie Endpoints
            var moviesGroup = apiGroup.MapGroup("/movies").RequireAuthorization();
            {
                moviesGroup.MapGet("/", MoviesEndpoints.GetAll);
                moviesGroup.MapGet("/{id:int}", MoviesEndpoints.GetById);
                moviesGroup.MapPost("/", MoviesEndpoints.Create);
                moviesGroup.MapPut("/{id:int}", MoviesEndpoints.Update);
                moviesGroup.MapDelete("/{id:int}", MoviesEndpoints.Delete);
            }

            // Theater Endpoints
            var theatersGroup = apiGroup.MapGroup("/theaters").RequireAuthorization();
            {
                theatersGroup.MapGet("/", TheatersEndpoints.GetAll);
                theatersGroup.MapGet("/{id:int}", TheatersEndpoints.GetById);
                theatersGroup.MapPost("/", TheatersEndpoints.Create);
                theatersGroup.MapPut("/{id:int}", TheatersEndpoints.Update);
                theatersGroup.MapDelete("/{id:int}", TheatersEndpoints.Delete);
            }
        }
    }
}
