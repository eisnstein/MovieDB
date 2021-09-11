using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieDB.Api.Entities;
using MovieDB.Api.Helpers;
using MovieDB.Shared.Models.Accounts;
using MovieDB.Api.Services;

namespace MovieDB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AccountResponse>> Authenticate(AuthenticateRequest model)
        {
            var response = await _accountService.AuthenticateAsync(model, GetIpAddress());
            SetRefreshTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { message = "Invalid refresh token" });
            }

            var response = await _accountService.RefreshTokenAsync(refreshToken, GetIpAddress());
            SetRefreshTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(RevokeTokenRequest model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required " });
            }

            if (!Account.OwnsToken(token) && Account.Role != Role.Admin)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            await _accountService.RevokeTokenAsync(token, GetIpAddress());

            return Ok(new { message = "Token revoked" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            await _accountService.RegisterAsync(model, Request.Headers["origin"]);
            return Ok(new { message = "Registration successful, please check your email for verification instructions" });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest model)
        {
            await _accountService.VerifyEmailAsync(model.Token);
            return Ok(new { message = "Verification successful, you can now login" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _accountService.ForgotPasswordAsync(model.Email, Request.Headers["origin"]);
            return Ok(new { message = "Please check your email for password reset instructions" });
        }

        [HttpPost("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken(ValidateResetTokenRequest model)
        {
            await _accountService.ValidateResetTokenAsync(model.Token);
            return Ok(new { message = "Token is valid"});
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            await _accountService.ResetPasswordAsync(model);
            return Ok(new { message = "Password reset successful, you can now login in" });
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AccountResponse>> GetById(int id)
        {
            if (Account is null || (id != Account.Id && Role.Admin != Account.Role))
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            var response = await _accountService.GetByIdAsync(id);

            return Ok(response);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<AccountResponse>> Update(int id, UpdateRequest model)
        {
            if (Account is null || (id != Account.Id && Role.Admin != Account.Role))
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            if (Account.Role != Role.Admin)
            {
                model.Role = null;
            }

            var response = await _accountService.UpdateAsync(id, model);

            return Ok(response);
        }

        // Helpers

        private void SetRefreshTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }

            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "no-ip-address";
        }
    }
}
