using Microsoft.AspNetCore.Http.HttpResults;
using MiniValidation;
using MovieDB.Api.App.Http.Requests;
using MovieDB.Api.App.Http.Responses;
using MovieDB.Api.App.Models;
using MovieDB.Api.App.Services;

namespace MovieDB.Api.App.Endpoints;

public static class AccountsEndpoint
{
    public static async Task<Results<ValidationProblem, BadRequest<ErrorMessage>, Ok<AuthenticateResponse>>> Authenticate(
        HttpContext context,
        IAccountService accountService,
        AuthenticateRequest model)
    {
        if (!MiniValidator.TryValidate(model, out var errors))
        {
            return TypedResults.ValidationProblem(errors);
        }

        var ipAddress = GetIpAddress(context);
        var (account, jwtToken, refreshToken) = await accountService.AuthenticateAsync(model, ipAddress);

        SetRefreshTokenCookie(context.Response, refreshToken);

        var response = new AuthenticateResponse()
        {
            Id = account.Id,
            Email = account.Email,
            IsVerified = account.IsVerified,
            Role = account.Role.ToString(),
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt,
            JwtToken = jwtToken
        };

        return TypedResults.Ok(response);
    }

    public static async Task<Results<Ok<RefreshTokenResponse>, BadRequest<object>>> RefreshToken(
        HttpContext context,
        IAccountService accountService)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return TypedResults.BadRequest<object>(new { message = "Invalid refresh token" });
        }

        var ipAddress = GetIpAddress(context);
        var (jwtToken, newRefreshToken) = await accountService.RefreshTokenAsync(refreshToken, ipAddress);
        SetRefreshTokenCookie(context.Response, newRefreshToken);

        var response = new RefreshTokenResponse()
        {
            JwtToken = jwtToken,
            RefreshToken = refreshToken
        };

        return TypedResults.Ok(response);
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

    public static async Task<Results<ValidationProblem, Ok<object>>> Register(
        HttpRequest request,
        IAccountService accountService,
        RegisterRequest model)
    {
        if (!MiniValidator.TryValidate(model, out var errors))
        {
            return TypedResults.ValidationProblem(errors);
        }

        await accountService.RegisterAsync(model, request.Headers["origin"]);

        return TypedResults.Ok<object>(new { message = "Registration successful, we sent you an email with verification instructions" });
    }

    public static async Task<Results<ValidationProblem, Ok<object>>> VerifyEmail(
        IAccountService accountService,
        VerifyEmailRequest model)
    {
        if (!MiniValidator.TryValidate(model, out var errors))
        {
            return TypedResults.ValidationProblem(errors);
        }

        await accountService.VerifyEmailAsync(model.Token!);
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
        if (context.Items[nameof(Account)] is not Account account || (account.Id != id && account.Role != Role.Admin))
        {
            return TypedResults.Unauthorized();
        }

        var accountById = await accountService.GetByIdAsync(id);

        return TypedResults.Ok(accountById.ToResponse());
    }

    public static async Task<Results<UnauthorizedHttpResult, Ok<AccountResponse>>> Update(
        HttpContext context,
        IAccountService accountService,
        int id,
        AccountUpdateRequest model)
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

        var updatedAccount = await accountService.UpdateAsync(id, model);

        return TypedResults.Ok(updatedAccount.ToResponse());
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
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var value))
        {
            return value.ToString();
        }

        return context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "no-ip-address";
    }
}
