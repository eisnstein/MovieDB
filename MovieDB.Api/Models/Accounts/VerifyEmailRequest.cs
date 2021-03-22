using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.Models.Accounts
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}