using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace VegEaseBackend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        private readonly string _fromEmail = "shah.tauheed02@gmail.com";
        private readonly string _fromName = "Raza Software and Technology";
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpServer = _configuration["Smtp:Host"];
            _smtpPort = Convert.ToInt32(_configuration["Smtp:Port"]);
            _smtpUser = _configuration["Smtp:Username"];
            _smtpPass = _configuration["Smtp:Password"];
        }

        public async Task SendEmailAsync(string recipientName, string recipientEmail, string subject)
        {
            string htmlBody = CreateEmailBody(recipientName);

            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(recipientEmail);

                await client.SendMailAsync(mailMessage);
            }
        }

        private string CreateEmailBody(string recipientName)
        {
            string htmlBody = @"<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8'><meta name='viewport' content='width=device-width,initial-scale=1'><title>Thank You!</title><style>body{font-family:Arial,sans-serif;background-color:#f4f4f4;margin:0;padding:0}.container{max-width:600px;margin:auto;background-color:#fff;padding:20px;border-radius:8px;box-shadow:0 0 10px rgba(0,0,0,.1)}h1{color:#333}p{color:#666;line-height:1.5}.footer{text-align:center;font-size:.9em;color:#999}</style></head><body><div class='container'><h1>Thank You!</h1><p>Dear [RecipientName],</p><p>Thank you for being a valued customer. We appreciate your business and look forward to serving you again in the future.</p><p>If you have any questions or need further assistance, feel free to reach out to us.</p><p>Best regards,</p><div class='footer'><p>&copy; [CurrentYear] Raza Software and Technology. All rights reserved.</p></div></div></body></html>";
            return htmlBody.Replace("[RecipientName]", recipientName).Replace("[CurrentYear]", DateTime.Now.Year.ToString());
        }
    }
}
