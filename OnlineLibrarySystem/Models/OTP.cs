using System.ComponentModel.DataAnnotations;

namespace OnlineLibrarySystem.Models
{
    public class OTP
    {
        [Required(ErrorMessage = "OTP is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be 6 digits long")]
        public string otp { get; set; }
    }
}
