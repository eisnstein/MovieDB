using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.Models.Accounts
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}