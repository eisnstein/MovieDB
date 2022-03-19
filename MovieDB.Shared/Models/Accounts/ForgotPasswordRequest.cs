using System.ComponentModel.DataAnnotations;

namespace MovieDB.Shared.Models.Accounts
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
    }
}
