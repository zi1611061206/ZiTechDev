using System.IO;

namespace ZiTechDev.Api.Engines.Email
{
    public class EmailTemplate
    {
        public string RootPath { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public EmailTemplate(string rootPath)
        {
            RootPath = rootPath;
            Header = File.OpenText(RootPath + @"\EmailTemplates\EmailHeaderTemplate.txt").ReadToEnd();
            Footer = File.OpenText(RootPath + @"\EmailTemplates\EmailFooterTemplate.txt").ReadToEnd();
            Header = Header.Replace("PLACEHOLDER-LINK-LOGO", "https://lh6.googleusercontent.com/EZSFUJ3Sa9pA8AY8AxZ4y3RPrm8QsGUZZHzWNEfDvxV8CQWeCpLMFIjyeBidIYLT8MLUcg=w16383");
            Footer = Footer.Replace("PLACEHOLDER-SIGN", "Zi-Team");
            Footer = Footer.Replace("PLACEHOLDER-LINK-HOME", "https://zitechdev.com");
        }

        public void EmailConfirmation(string confirmUrl)
        {
            Subject = "[ZITECHDEV] Xác thực Email";
            Body = File.OpenText(RootPath + @"\EmailTemplates\EmailConfirmationTemplate.txt").ReadToEnd();

            Header = Header.Replace("PLACEHOLDER-TITLE", "XÁC THỰC EMAIL");
            Header = Header.Replace("PLACEHOLDER-DEAR", "Chúng tôi rất vui khi có sự đồng hành của bạn. Vui lòng nhấn vào nút bên dưới để kích hoạt tài khoản của bạn");

            Body = Body.Replace("PLACEHOLDER-LINK-CONFIRM", confirmUrl);

            Content = Header + Body + Footer;
        }
    }
}
