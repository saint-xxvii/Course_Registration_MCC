using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace sample_1.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpSettings = _config.GetSection("SmtpSettings");

            var message = new MailMessage
            {
                From = new MailAddress(smtpSettings["SenderEmail"], smtpSettings["SenderName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            using (var client = new SmtpClient(smtpSettings["Server"], int.Parse(smtpSettings["Port"])))
            {
                client.Credentials = new NetworkCredential(
                    smtpSettings["Username"],
                    smtpSettings["Password"]
                );
                client.EnableSsl = true;  // Gmail requires SSL

                await client.SendMailAsync(message);
            }
        }
    }
}
