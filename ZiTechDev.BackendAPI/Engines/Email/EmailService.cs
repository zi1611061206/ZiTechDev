using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZiTechDev.Business.Engines.Email;

namespace ZiTechDev.BackendAPI.Engines.Email
{
    public class EmailService : IEmailService
    {
        private readonly IEmailServerConfiguration _emailServerConfiguration;

        public EmailService(IEmailServerConfiguration emailServerConfiguration)
        {
            _emailServerConfiguration = emailServerConfiguration;
        }

        public void Send(EmailItem emailItem)
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
            // Khuyến nghị để SSL = true
            emailClient.Connect(_emailServerConfiguration.SmtpServer, _emailServerConfiguration.SmtpPort, true);
            // Xóa bất kỳ chức năng OAuth nào vì chúng tôi sẽ không sử dụng chức năng đó.
            emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
            emailClient.Authenticate(_emailServerConfiguration.SmtpUsername, _emailServerConfiguration.SmtpPassword);
            emailClient.Send(email);
            emailClient.Disconnect(true);
        }

        public List<EmailItem> Receive(int maxCount = 10)
        {
            using var emailClient = new Pop3Client();
            emailClient.Connect(_emailServerConfiguration.PopServer, _emailServerConfiguration.PopPort, true);
            emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
            emailClient.Authenticate(_emailServerConfiguration.PopUsername, _emailServerConfiguration.PopPassword);

            List<EmailItem> emails = new();
            for (int i = 0; i < emailClient.Count && i < maxCount; i++)
            {
                var email = emailClient.GetMessage(i);
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
