using Microsoft.Extensions.Configuration;
using Resend;
using System.Net.Mail;

namespace TalStorage.Service
{
    public class EmailService : IEmailService
    {
        private readonly IResend _resend;
        private readonly IConfiguration _configuration;

        public EmailService(IResend resend, IConfiguration configuration)
        {
            _resend = resend;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new EmailMessage
            {
                From = _configuration["Resend:FromEmail"],
                To = to,
                Subject = subject,
                HtmlBody = body
            };
            try
            {
                await _resend.EmailSendAsync(message);
            }
            catch
            {
                
            }
        }
    }
}
