using System.ComponentModel.DataAnnotations;

namespace SixteenClothing.ViewModels.UserViewModels
{
    public class LoginVM
    {
     

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(256), MinLength(6), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

    }
}
