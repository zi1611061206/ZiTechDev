using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.Email;

namespace ZiTechDev.Api.EmailConfiguration
{
    public class EmailService : IEmailService
    {
        private readonly IEmailServerConfiguration _emailServerConfiguration;

        public EmailService(IEmailServerConfiguration emailServerConfiguration)
        {
            _emailServerConfiguration = emailServerConfiguration;
        }

        public async Task<bool> SendAsync(EmailItem emailItem)
        {
            var email = new MimeMessage();
            email.From.AddRange(emailItem.Senders.Select(x => new MailboxAddress(x.Name, x.Address)));
            email.To.AddRange(emailItem.Receivers.Select(x => new MailboxAddress(x.Name, x.Address)));
            email.Subject = emailItem.Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = emailItem.Body
            };

            using var emailClient = new SmtpClient();
            emailClient.CheckCertificateRevocation = false;
            // Ghi đè phiên bản ssl/tls: emailClient.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
            // Khuyến nghị để SSL = true
            await emailClient.ConnectAsync(_emailServerConfiguration.SmtpServer, _emailServerConfiguration.SmtpPort, true);
            // Xóa bất kỳ chức năng OAuth nào vì chúng tôi sẽ không sử dụng chức năng đó.
            emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
            //Turn on to test: https://myaccount.google.com/lesssecureapps?pli=1&rapt=AEjHL4MPFGcGo6RugIy9aEUcS9EhTk0ZF_1Mo4dgnC9zr9D6GDypFGzJBhXmzUAiQm_FG0bJ7LkzC_i2Ca5n0UhbJwXQ5SxpzA
            await emailClient.AuthenticateAsync(_emailServerConfiguration.SmtpUsername, _emailServerConfiguration.SmtpPassword);
            await emailClient.SendAsync(email);
            await emailClient.DisconnectAsync(true);
            return true;
        }

        public async Task<List<EmailItem>> ReceiveAsync(int maxCount = 10)
        {
            using var emailClient = new Pop3Client();
            await emailClient.ConnectAsync(_emailServerConfiguration.PopServer, _emailServerConfiguration.PopPort, true);
            emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
            await emailClient.AuthenticateAsync(_emailServerConfiguration.PopUsername, _emailServerConfiguration.PopPassword);

            List<EmailItem> emails = new List<EmailItem>();
            for (int i = 0; i < emailClient.Count && i < maxCount; i++)
            {
                var email = await emailClient.GetMessageAsync(i);
                var emailItem = new EmailItem()
                {
                    Subject = email.Subject,
                    Body = !string.IsNullOrEmpty(email.HtmlBody) ? email.HtmlBody : email.TextBody,
                };
                emailItem.Senders.AddRange(email.From.Select(x => (MailboxAddress)x).Select(x => new EmailBase(x.Name, x.Address)));
                emailItem.Receivers.AddRange(email.To.Select(x => (MailboxAddress)x).Select(x => new EmailBase(x.Name, x.Address)));
                emails.Add(emailItem);
            }
            return emails;
        }
    }
}
