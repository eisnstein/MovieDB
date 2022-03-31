using MovieDB.Api.App.Entities;
using MovieDB.Shared.Models.Accounts;
using MovieDB.Api.App.Services;

namespace MovieDB.Api.App.Endpoints;

public static class AccountsEndpoint
{
    public static async Task<IResult> Authenticate(
        HttpContext context,
        IAccountService accountService,
        AuthenticateRequest model)
    {
        var ipAddress = GetIpAddress(context);
        var responseData = await accountService.AuthenticateAsync(model, ipAddress);
        SetRefreshTokenCookie(context.Response, responseData.RefreshToken);

        return Results.Ok(responseData);
    }

    public static async Task<IResult> RefreshToken(
        HttpContext context,
        IAccountService accountService)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Results.BadRequest(new { message = "Invalid refresh token" });
        }

        var ipAddress = GetIpAddress(context);
        var responseData = await accountService.RefreshTokenAsync(refreshToken, ipAddress);
        SetRefreshTokenCookie(context.Response, responseData.RefreshToken);

        return Results.Ok(responseData);
    }

    public static async Task<IResult> RevokeToken(
        HttpContext context,
        IAccountService accountService,
        RevokeTokenRequest model)
    {
        var token = model.Token ?? context.Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
        {
            return Results.BadRequest(new { message = "Token is required " });
        }

        if (context.Items["Account"] is Account account && !account.OwnsToken(token) && account.Role != Role.Admin)
        {
            return Results.Unauthorized();
        }

        var ipAddress = GetIpAddress(context);
        await accountService.RevokeTokenAsync(token, ipAddress);

        return Results.Ok(new { message = "Token revoked" });
    }

    public static async Task<IResult> Register(
        HttpRequest request,
        IAccountService accountService,
        RegisterRequest model)
    {
        await accountService.RegisterAsync(model, request.Headers["origin"]);
        return Results.Ok(new { message = "Registration successful, please check your email for verification instructions" });
    }

    public static async Task<IResult> VerifyEmail(
        IAccountService accountService,
        VerifyEmailRequest model)
    {
        await accountService.VerifyEmailAsync(model.Token);
        return Results.Ok(new { message = "Verification successful, you can now login" });
    }

    public static async Task<IResult> ForgotPassword(
        HttpRequest request,
        IAccountService accountService,
        ForgotPasswordRequest model)
    {
        await accountService.ForgotPasswordAsync(model.Email, request.Headers["origin"]);
        return Results.Ok(new { message = "Please check your email for password reset instructions" });
    }

    public static async Task<IResult> ValidateResetToken(
        IAccountService accountService,
        ValidateResetTokenRequest model)
    {
        await accountService.ValidateResetTokenAsync(model.Token);
        return Results.Ok(new { message = "Token is valid"});
    }

    public static async Task<IResult> ResetPassword(
        IAccountService accountService,
        ResetPasswordRequest model)
    {
        await accountService.ResetPasswordAsync(model);
        return Results.Ok(new { message = "Password reset successful, you can now login in" });
    }

    public static async Task<IResult> GetById(
        HttpContext context,
        IAccountService accountService,
        int id)
    {
        var account = context.Items[nameof(Account)] as Account;
        if (account.Id != id && account.Role != Role.Admin)
        {
            return Results.Unauthorized();
        }

        var responseData = await accountService.GetByIdAsync(id);

        return Results.Ok(responseData);
    }

    public static async Task<IResult> Update(
        HttpContext context,
        IAccountService accountService,
        int id,
        UpdateRequest model)
    {
        // We can be sure that the Account is set here because we already checked that in the Authorization
        var account = (Account) context.Items[nameof(Account)]!;
        if (id != account.Id && Role.Admin != account.Role)
        {
            return Results.Unauthorized();
        }

        if (account.Role != Role.Admin)
        {
            model.Role = null;
        }

        var responseData = await accountService.UpdateAsync(id, model);

        return Results.Ok(responseData);
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
