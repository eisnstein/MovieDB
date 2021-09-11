using System;
using System.Text.Json.Serialization;

namespace MovieDB.Shared.Models.Accounts
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
