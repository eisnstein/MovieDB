using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieDB.Api.Entities;
using MovieDB.Api.Helpers;
using MovieDB.Api.Models.Accounts;

namespace MovieDB.Api.Services
{
    public interface IAccountService
    {
        public Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress);
        public Task RegisterAsync(RegisterRequest model, string? origin);
        public Task VerifyEmailAsync(string token);
    }

    public class AccountService : IAccountService
    {
        private readonly AppDbContext _db;
        private readonly IAppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;

        public AccountService(
            AppDbContext db,
            IAppSettings appSettings,
            IMapper mapper,
            ILogger<AccountService> logger,
            IEmailService emailService)
        {
            _db = db;
            _appSettings = appSettings;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress)
        {
            var account = await _db.Accounts.SingleOrDefaultAsync(a => a.Email == model.Email);

            if (account is null || !account.IsVerified || !BCrypt.Net.BCrypt.Verify(model.Password, account.PasswordHash))
            {
                throw new AppException("Email or password incorrect");
            }

            var jwtToken = GenerateJwtToken(account);
            var refreshToken = GenerateRefreshToken(ipAddress);

            account.RefreshTokens?.Add(refreshToken);
            RemoveOldRefreshTokens(account);

            await _db.SaveChangesAsync();

            var response = _mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = refreshToken.Token;
            return response;
        }

        public async Task RegisterAsync(RegisterRequest model, string? origin)
        {
            var emailExists = await _db.Accounts.AnyAsync(a => a.Email == model.Email);
            if (emailExists)
            {
                await SendAlreadyRegisteredEmailAsync(model.Email, origin);
                return;
            }

            var account = _mapper.Map<Account>(model);
            account.Role = Role.User;
            account.CreatedAt = DateTime.UtcNow;
            account.VerificationToken = RandomTokenString();
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();

            await SendVerificationEmail(account, origin);
        }

        public async Task VerifyEmailAsync(string token)
        {
            var account = await _db.Accounts.SingleOrDefaultAsync(a => a.VerificationToken == token);
            if (account is null)
            {
                throw new AppException("Verification failed");
            }

            account.VerificationToken = null;
            account.Verified = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        // Helpers

        private async Task<Account> GetAccount(int id)
        {
            var account = await _db.Accounts.FindAsync(id);
            if (account is null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            return account;
        }

        private async Task<(RefreshToken, Account)> GetRefreshToken(string token)
        {
            var account = await _db.Accounts.SingleOrDefaultAsync(a => a.RefreshTokens != null && a.RefreshTokens.Any(r => r.Token == token));
            if (account is null)
            {
                throw new AppException("Invalid token");
            }

            var refreshToken = account.RefreshTokens?.Single(t => t.Token == token);
            if (refreshToken is null || !refreshToken.IsActive)
            {
                throw new AppException("Invalid token");
            }

            return (refreshToken, account);
        }

        private string GenerateJwtToken(Account account)
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(_appSettings.RefreshTokenTtl),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        // Emails

        private async Task SendVerificationEmail(Account account, string? origin)
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

            await _emailService.SendAsync(
                to: account.Email,
                subject: "Sign-up Verification API - Verify Email",
                html: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}"
            );
        }

        private async Task SendAlreadyRegisteredEmailAsync(string email, string? origin)
        {
            string message = string.IsNullOrEmpty(origin)
                ? "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>"
                : $@"<p>If you don't know your password please visit the <a href=""{origin}/accounts/forgot-password"">forgot password</a> page.</p>";

            await _emailService.SendAsync(
                to: email,
                subject: "Sign-Up Verification API - Email Already Registered",
                html: $@"<h4>Email Already Registered</h4>
                        <p>Your email <strong>{email}</strong> is already registered.</p>
                        {message}"
            );
        }
    }
}