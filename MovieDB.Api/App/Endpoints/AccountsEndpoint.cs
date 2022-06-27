using Microsoft.AspNetCore.Http.HttpResults;
using MiniValidation;
using MovieDB.Api.App.Entities;
using MovieDB.Shared.Models.Accounts;
using MovieDB.Api.App.Services;

namespace MovieDB.Api.App.Endpoints;

public static class AccountsEndpoint
{
    public static async Task<Results<ValidationProblem, Ok<AuthenticateResponse>>> Authenticate(
        HttpContext context,
        IAccountService accountService,
        AuthenticateRequest model)
    {
        if (!MiniValidator.TryValidate(model, out var errors))
        {
            return TypedResults.ValidationProblem(errors);
        }

        var ipAddress = GetIpAddress(context);
        var responseData = await accountService.AuthenticateAsync(model, ipAddress);
        SetRefreshTokenCookie(context.Response, responseData.RefreshToken);

        return TypedResults.Ok(responseData);
    }

    public static async Task<Results<Ok<AuthenticateResponse>, BadRequest<object>>> RefreshToken(
        HttpContext context,
        IAccountService accountService)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return TypedResults.BadRequest<object>(new { message = "Invalid refresh token" });
        }

        var ipAddress = GetIpAddress(context);
        var responseData = await accountService.RefreshTokenAsync(refreshToken, ipAddress);
        SetRefreshTokenCookie(context.Response, responseData.RefreshToken);

        return TypedResults.Ok(responseData);
    }

    public static async Task<Results<Ok<object>, UnauthorizedHttpResult, BadRequest<object>>> RevokeToken(
        HttpContext context,
        IAccountService accountService,
        RevokeTokenRequest model)
    {
        var token = model.Token ?? context.Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
        {
            return TypedResults.BadRequest<object>(new { message = "RefreshToken is required" });
        }

        if (context.Items["Account"] is Account account && !account.OwnsToken(token) && account.Role != Role.Admin)
        {
            return TypedResults.Unauthorized();
        }

        var ipAddress = GetIpAddress(context);
        await accountService.RevokeTokenAsync(token, ipAddress);

        return TypedResults.Ok<object>(new { message = "Token successfully revoked" });
    }

    public static async Task<Ok<object>> Register(
        HttpRequest request,
        IAccountService accountService,
        RegisterRequest model)
    {
        await accountService.RegisterAsync(model, request.Headers["origin"]);
        return TypedResults.Ok<object>(new { message = "Registration successful, we sent you an email with verification instructions" });
    }

    public static async Task<Ok<object>> VerifyEmail(
        IAccountService accountService,
        VerifyEmailRequest model)
    {
        await accountService.VerifyEmailAsync(model.Token);
        return TypedResults.Ok<object>(new { message = "Verification successful, you can now login" });
    }

    public static async Task<Ok<object>> ForgotPassword(
        HttpRequest request,
        IAccountService accountService,
        ForgotPasswordRequest model)
    {
        await accountService.ForgotPasswordAsync(model.Email, request.Headers["origin"]);
        return TypedResults.Ok<object>(new { message = "Please check your email for password reset instructions" });
    }

    public static async Task<Ok<object>> ValidateResetToken(
        IAccountService accountService,
        ValidateResetTokenRequest model)
    {
        await accountService.ValidateResetTokenAsync(model.Token);
        return TypedResults.Ok<object>(new { message = "Token is valid"});
    }

    public static async Task<Ok<object>> ResetPassword(
        IAccountService accountService,
        ResetPasswordRequest model)
    {
        await accountService.ResetPasswordAsync(model);
        return TypedResults.Ok<object>(new { message = "Password reset successful, you can now login in" });
    }

    public static async Task<Results<UnauthorizedHttpResult, Ok<AccountResponse>>> GetById(
        HttpContext context,
        IAccountService accountService,
        int id)
    {
        var account = context.Items[nameof(Account)] as Account;
        if (account.Id != id && account.Role != Role.Admin)
        {
            return TypedResults.Unauthorized();
        }

        var responseData = await accountService.GetByIdAsync(id);

        return TypedResults.Ok(responseData);
    }

    public static async Task<Results<UnauthorizedHttpResult, Ok<AccountResponse>>> Update(
        HttpContext context,
        IAccountService accountService,
        int id,
        UpdateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        if (id != account.Id && Role.Admin != account.Role)
        {
            return TypedResults.Unauthorized();
        }

        if (account.Role != Role.Admin)
        {
            model.Role = null;
        }

        var responseData = await accountService.UpdateAsync(id, model);

        return TypedResults.Ok(responseData);
    }

    // Helpers

    private static void SetRefreshTokenCookie(HttpResponse response, string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private static string GetIpAddress(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return context.Request.Headers["X-Forwarded-For"];
        }

        return context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "no-ip-address";
    }
}
