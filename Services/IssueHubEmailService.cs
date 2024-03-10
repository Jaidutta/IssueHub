using IssueHub.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
namespace IssueHub.Services
{
    public class IssueHubEmailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        public IssueHubEmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(string emailTo, string subject, string htmlMessage)
        {  // We will need MimeKit and MailKit package
            MimeMessage email = new();

            email.Sender = MailboxAddress.Parse(_mailSettings.Mail); // setting sender
            email.To.Add(MailboxAddress.Parse(emailTo)); // setting who we will send the mail to
            email.Subject = subject; // setting subject


            /// BodyBuilder helps us to build the body of our email as an HTML
            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage 

            };

            email.Body = builder.ToMessageBody(); // format the email body and set it to email.Body 

            try
            {
                using var smtp = new SmtpClient(); //  MailKit.Net.Smtp;
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls); // MailKit.Security
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

                await smtp.SendAsync(email);

                smtp.Disconnect(true);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
