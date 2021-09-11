using System.ComponentModel.DataAnnotations;

namespace MovieDB.Shared.Models.Accounts
{
    public class UpdateRequest
    {
        private string? _password;
        private string? _confirmPassword;
        private string? _role;
        private string? _email;

        public string? Role
        {
            get => _role;
            set => _role = replaceEmptyWithNull(value);
        }

        [EmailAddress]
        public string? Email
        {
            get => _email;
            set => _email = replaceEmptyWithNull(value);
        }

        [MinLength(8)]
        public string? Password
        {
            get => _password;
            set => _password = replaceEmptyWithNull(value);
        }

        [Compare(nameof(Password))]
        public string? ConfirmPassword
        {
            get => _confirmPassword;
            set => _confirmPassword = replaceEmptyWithNull(value);
        }

        private string? replaceEmptyWithNull(string? value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}
