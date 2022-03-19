using System.ComponentModel.DataAnnotations;

namespace MovieDB.Shared.Models.Accounts
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; } = default!;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = default!;

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = default!;
    }
}
