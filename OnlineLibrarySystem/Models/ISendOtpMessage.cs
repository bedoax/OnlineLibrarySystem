using System.Threading.Tasks;

namespace OnlineLibrarySystem.Models
{
    public interface ISendOtpMessage
    {
        Task SendEmailAsync(string email, string subject, string otp);
    }
}
