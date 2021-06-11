using System.IO;

namespace ZiTechDev.CommonModel.Engines.Email
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
            Header = Header.Replace("PLACEHOLDER-LINK-LOGO", "https://drive.google.com/file/d/1PuSFxtEUQ_D5P2au4XuXMJagejJWd2fg/view");
            Footer = Footer.Replace("PLACEHOLDER-SIGN", "Zi-Team");
            Footer = Footer.Replace("PLACEHOLDER-LINK-HOME", "https://zitechdev.com");
        }

        public void EmailConfirmation(string confirmUrl, string userName)
        {
            Body = File.OpenText(RootPath + @"\EmailTemplates\EmailConfirmationBodyTemplate.txt").ReadToEnd();
            Subject = "[ZITECHDEV] Thông báo xác thực Email";
            Header = Header.Replace("PLACEHOLDER-TITLE", "XÁC THỰC EMAIL");
            Header = Header.Replace("PLACEHOLDER-DEAR", $"Chào {userName}." +
                $" Vui lòng nhấn vào nút bên dưới để xác minh địa chỉ email của bạn.");
            Body = Body.Replace("PLACEHOLDER-LINK-CONFIRM", confirmUrl);
            Content = Header + Body + Footer;
        }

        public void ForgotPassword(string url, string userName)
        {
            Body = File.OpenText(RootPath + @"\EmailTemplates\ForgotPasswordBodyTemplate.txt").ReadToEnd();
            Subject = "[ZITECHDEV] Thông báo đổi mật khẩu";
            Header = Header.Replace("PLACEHOLDER-TITLE", "QUÊN MẬT KHẨU");
            Header = Header.Replace("PLACEHOLDER-DEAR", $"Chào {userName}." +
                $" Có phải bạn vừa thực hiện yêu cầu thay đổi mật khẩu do quên mật khẩu cũ?" +
                $" Nếu đúng, vui lòng nhấn nút bên dưới để tiếp tục.");
            Body = Body.Replace("PLACEHOLDER-LINK-CONFIRM", url);
            Content = Header + Body + Footer;
        }
    }
}
