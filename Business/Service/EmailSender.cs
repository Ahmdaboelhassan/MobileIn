using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Business.Service
{
    public class EmailSender : IEmailSender
    {
        readonly EmailSettings _emailSettings;
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
          //  _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //var mimeEmail = new MimeMessage();
            //mimeEmail.From.Add(new MailboxAddress(_emailSettings.displayName,_emailSettings.email));
            //mimeEmail.To.Add(MailboxAddress.Parse(email));
            //mimeEmail.Subject = subject;

            //// body
            //var builder = new BodyBuilder();
            //builder.HtmlBody = htmlMessage;
            //mimeEmail.Body = builder.ToMessageBody();

            //using (var smtp = new SmtpClient())
            //{
            //    smtp.Connect(_emailSettings.host, _emailSettings.port,SecureSocketOptions.StartTls);
            //    smtp.Authenticate(_emailSettings.email,_emailSettings.password);
            //    await smtp.SendAsync(mimeEmail);
            //    smtp.Disconnect(true);
            //}
        }
    }
}
