using System.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieDB.Api.App.Helpers;
using MovieDB.Api.App.Http.Requests;
using MovieDB.Api.App.Models;

namespace MovieDB.Api.App.Services;

public interface IAccountService
{
    public Task<(Account account, string jwtToken, string refreshToken)> AuthenticateAsync(AuthenticateRequest model, string ipAddress);
    public Task<(string jwtToken, string refreshToken)> RefreshTokenAsync(string refreshToken, string ipAddress);
    public Task RevokeTokenAsync(string token, string ipAddress);
    public Task RegisterAsync(RegisterRequest model, string? origin);
    public Task VerifyEmailAsync(string token);
    public Task ForgotPasswordAsync(string email, string? origin);
    public Task ValidateResetTokenAsync(string token);
    public Task ResetPasswordAsync(ResetPasswordRequest model);
    public Task<Account> GetByIdAsync(int id);
    public Task<Account> UpdateAsync(int id, AccountUpdateRequest model);
}

public class AccountService : IAccountService
{
    private readonly AppDbContext _db;
    private readonly AppSettings _appSettings;
    private readonly IEmailService _emailService;

    public AccountService(
        AppDbContext db,
        IOptions<AppSettings> appSettings,
        IEmailService emailService)
    {
        _db = db;
        _appSettings = appSettings.Value;
        _emailService = emailService;
    }

    public async Task<(Account account, string jwtToken, string refreshToken)> AuthenticateAsync(AuthenticateRequest model, string ipAddress)
    {
        var account = await _db.Accounts.SingleOrDefaultAsync(a => a.Email == model.Email);
        if (account is null || !account.IsVerified || !BC.Verify(model.Password, account.PasswordHash))
        {
            throw new AppException("Email or password wrong");
        }

        var jwtToken = GenerateJwtToken(account);
        var refreshToken = GenerateRefreshToken(ipAddress);

        account.RefreshTokens?.Add(refreshToken);
        RemoveOldRefreshTokens(account);

        await _db.SaveChangesAsync();

        return (account, jwtToken, refreshToken.Token);
    }

    public async Task<(string jwtToken, string refreshToken)> RefreshTokenAsync(string token, string ipAddress)
    {
        var (refreshToken, account) = await GetRefreshTokenAsync(token);

        var newRefreshToken = GenerateRefreshToken(ipAddress);
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReplacedByToken = newRefreshToken.Token;

        account.RefreshTokens?.Add(newRefreshToken);
        RemoveOldRefreshTokens(account);

        await _db.SaveChangesAsync();

        var jwtToken = GenerateJwtToken(account);

        return (jwtToken, refreshToken.Token);
    }

    public async Task RevokeTokenAsync(string token, string ipAddress)
    {
        var (refreshToken, _) = await GetRefreshTokenAsync(token);

        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;

        await _db.SaveChangesAsync();
    }

    public async Task RegisterAsync(RegisterRequest model, string? origin)
    {
        var emailExists = await _db.Accounts.AnyAsync(a => a.Email == model.Email);
        if (emailExists)
        {
            SendAlreadyRegisteredEmail(model.Email!, origin);
            return;
        }

        var account = new Account()
        {
            Email = model.Email!,
            Role = Role.User,
            CreatedAt = DateTime.UtcNow,
            VerificationToken = RandomTokenString(),
            PasswordHash = BC.HashPassword(model.Password),
        };

        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();

        SendVerificationEmail(account, origin);
    }

    public async Task VerifyEmailAsync(string token)
    {
        var account = await _db.Accounts.SingleOrDefaultAsync(a => a.VerificationToken == token);
        if (account is null)
        {
            throw new AppException("Verification failed");
        }

        account.VerificationToken = null;
        account.VerifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task ForgotPasswordAsync(string email, string? origin)
    {
        var account = await _db.Accounts.SingleOrDefaultAsync(a => a.Email == email);
        if (account is null)
        {
            return;
        }

        account.ResetToken = RandomTokenString();
        account.ResetTokenExpiresAt = DateTime.UtcNow.AddDays(1);

        await _db.SaveChangesAsync();
        SendPasswordResetEmail(account, origin);
    }

    public async Task ValidateResetTokenAsync(string token)
    {
        var account = await _db.Accounts.SingleOrDefaultAsync(a =>
            a.ResetToken == token &&
            a.ResetTokenExpiresAt > DateTime.UtcNow);
        if (account is null)
        {
            throw new AppException("Token invalid");
        }
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest model)
    {
        var account = await _db.Accounts.SingleOrDefaultAsync(a =>
            a.ResetToken == model.Token &&
            a.ResetTokenExpiresAt > DateTime.UtcNow);
        if (account is null)
        {
            throw new AppException("Token invalid");
        }

        account.PasswordHash = BC.HashPassword(model.Password);
        account.ResetToken = null;
        account.ResetTokenExpiresAt = null;

        await _db.SaveChangesAsync();
    }

    public Task<Account> GetByIdAsync(int id)
    {
        return GetAccountAsync(id);
    }

    public async Task<Account> UpdateAsync(int id, AccountUpdateRequest model)
    {
        var account = await GetAccountAsync(id);
        if (model.Email != account.Email && await _db.Accounts.AnyAsync(a => a.Email == model.Email))
        {
            throw new AppException($"Email '{model.Email}' is already taken");
        }

        if (!string.IsNullOrEmpty(model.Password))
        {
            if (model.Password != model.ConfirmPassword)
            {
                throw new AppException($"Passwords don't match");
            }

            account.PasswordHash = BC.HashPassword(model.Password);
        }

        var role = account.Role;
        if (Enum.TryParse(model.Role, out Role roleResult))
        {
            role = roleResult;
        }

        account.Email = model.Email ?? account.Email;
        account.Role = role;
        account.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return account;
    }

    // Helpers

    private async Task<Account> GetAccountAsync(int id)
    {
        var account = await _db.Accounts.FindAsync(id);
        if (account is null)
        {
            throw new KeyNotFoundException("Account not found");
        }

        return account;
    }

    private async Task<(RefreshToken, Account)> GetRefreshTokenAsync(string token)
    {
        var account = await _db.Accounts.SingleOrDefaultAsync(a => a.RefreshTokens != null && a.RefreshTokens.Any(r => r.Token == token));
        if (account is null)
        {
            throw new AppException("Invalid token");
        }

        var refreshToken = account.RefreshTokens!.Single(t => t.Token == token);
        if (refreshToken is null || !refreshToken.IsActive)
        {
            throw new AppException("Invalid token");
        }

        return (refreshToken, account);
    }

    private string GenerateJwtToken(Account account)
    {
        return Jwt.BuildToken(account, _appSettings.Secret);
    }

    private RefreshToken GenerateRefreshToken(string ipAddress)
    {
        return new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }

    private void RemoveOldRefreshTokens(Account account)
    {
        account.RefreshTokens?.RemoveAll(r =>
            !r.IsActive &&
            r.CreatedAt.AddDays(_appSettings.RefreshTokenTtl) <= DateTime.UtcNow);
    }

    private string RandomTokenString()
    {
        using var rngCryptoServiceProvider = RandomNumberGenerator.Create();
        var randomBytes = new byte[40];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        return BitConverter.ToString(randomBytes).Replace("-", "");
    }

    // Emails

    private void SendVerificationEmail(Account account, string? origin)
    {
        string message;
        if (string.IsNullOrEmpty(origin))
        {
            message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                         <p><code>{account.VerificationToken}</code></p>";
        }
        else
        {
            var verifyUrl = $"{origin}/accounts/verify-email?token={account.VerificationToken}";
            message = $@"<p>Please click the below link to verify your email address:</p>
                         <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
        }

        _emailService.SendAsync(
            to: account.Email,
            subject: "Sign-up Verification API - Verify Email",
            html: $@"<h4>Verify Email</h4>
                     <p>Thanks for registering!</p>
                     {message}"
        );
    }

    private void SendAlreadyRegisteredEmail(string email, string? origin)
    {
        string message = string.IsNullOrEmpty(origin)
            ? "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>"
            : $@"<p>If you don't know your password please visit the <a href=""{origin}/accounts/forgot-password"">forgot password</a> page.</p>";

        _emailService.SendAsync(
            to: email,
            subject: "Sign-Up Verification API - Email Already Registered",
            html: $@"<h4>Email Already Registered</h4>
                    <p>Your email <strong>{email}</strong> is already registered.</p>
                    {message}"
        );
    }

    private void SendPasswordResetEmail(Account account, string? origin)
    {
        string message;
        if (string.IsNullOrEmpty(origin))
        {
            message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                         <p><code>{account.ResetToken}</code></p>";
        }
        else
        {
            var resetUrl = $"{origin}/account/reset-password?token={account.ResetToken}";
            message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                         <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
        }

        _emailService.SendAsync(
            to: account.Email,
            subject: "Sign-up Verification API - Reset Password",
            html: $@"<h4>Reset Password Email</h4>
                     {message}"
        );
    }
}
