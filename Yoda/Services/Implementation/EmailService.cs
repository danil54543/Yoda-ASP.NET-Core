using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
namespace Yoda.Services.Implementation
{
    public class EmailService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("yoda.app@ukr.net", "yoda.app@ukr.net"));
            emailMessage.To.Add(new MailboxAddress(email, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.ukr.net", 465, true);
            await client.AuthenticateAsync("yoda.app@ukr.net", "6oXEN4pF67qMdP2t");
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
