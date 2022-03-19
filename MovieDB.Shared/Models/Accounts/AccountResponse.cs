using System;

namespace MovieDB.Shared.Models.Accounts
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsVerified { get; set; }
    }
}
