using Documents.Application.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace Documents.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _email;
        private readonly string _passcode;

        public EmailService(IConfiguration configuration)
        {
            _email = configuration["EmailService:Email"]
                ?? throw new ArgumentNullException("Email.Email not found.");

            _passcode = configuration["EmailService:Passcode"]
                ?? throw new ArgumentNullException("Email.Passcode not found.");
        }

        public async Task SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken)
        {
            using (var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_email, _passcode),
                EnableSsl = true
            })
            {
                var token = Guid.NewGuid().ToString();
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_email),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(email);
                await smtpClient.SendMailAsync(mailMessage, cancellationToken);
            }
        }
    }
}
