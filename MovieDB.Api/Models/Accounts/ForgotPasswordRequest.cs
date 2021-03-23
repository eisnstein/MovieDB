using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.Models.Accounts
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}