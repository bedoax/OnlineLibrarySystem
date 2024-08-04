using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OnlineLibrarySystem.Models
{
    public class SendOtpMessage : ISendOtpMessage
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var myemail = "mytestapi@outlook.com";
            var mypw = "1542001aA@"; // Use an app password for better security
            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(myemail,mypw)
            };
            return client.SendMailAsync(
                new MailMessage(from: myemail,
                to: email,
                subject,
                message));
        }
    }
}
