using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibrarySystem.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Username or Email is required")]
        [DisplayName("Username or Email")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
