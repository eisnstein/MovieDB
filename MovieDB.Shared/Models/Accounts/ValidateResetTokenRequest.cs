using System.ComponentModel.DataAnnotations;

namespace MovieDB.Shared.Models.Accounts
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; } = default!;
    }
}
