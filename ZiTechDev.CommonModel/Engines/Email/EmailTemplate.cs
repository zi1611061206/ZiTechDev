using System;
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

        public void LoginWarning(string forgotPasswordUrl, string userName)
        {
            Body = File.OpenText(RootPath + @"\EmailTemplates\LoginWarningBodyTemplate.txt").ReadToEnd();
            Subject = "[ZITECHDEV] Cảnh báo đăng nhập";
            Header = Header.Replace("PLACEHOLDER-TITLE", "CẢNH BÁO ĐĂNG NHẬP");
            Header = Header.Replace("PLACEHOLDER-DEAR", $"Chào {userName}." +
                $" Tài khoản của bạn sẽ bị khóa tạm thời trong 15 phút do đăng nhập sai thông tin quá nhiều." +
                $" Nếu bạn quên mật khẩu hoặc nghi ngờ tài khoản của bạn bị tấn công," +
                $" vui lòng nhấn nút bên dưới để tiến hành đổi mật khẩu.");
            Body = Body.Replace("PLACEHOLDER-LINK-FORGOT", forgotPasswordUrl);
            Content = Header + Body + Footer;
        }

        public void ChangeEmailConfirmation(string changeEmailUrl, string userName)
        {
            Body = File.OpenText(RootPath + @"\EmailTemplates\ChangeEmailConfirmationBodyTemplate.txt").ReadToEnd();
            Subject = "[ZITECHDEV] Thông báo thay đổi Email đăng ký";
            Header = Header.Replace("PLACEHOLDER-TITLE", "THAY ĐỔI EMAIL");
            Header = Header.Replace("PLACEHOLDER-DEAR", $"Chào {userName}." +
                $" Vui lòng nhấn vào nút bên dưới để xác minh địa chỉ email mới của bạn.");
            Body = Body.Replace("PLACEHOLDER-LINK-CONFIRM", changeEmailUrl);
            Content = Header + Body + Footer;
        }

        public void Authenticate2FA(string userName, string code)
        {
            Subject = "[ZITECHDEV] Xác thực 2 bước";
            Header = Header.Replace("PLACEHOLDER-TITLE", "XÁC THỰC 2 BƯỚC");
            Header = Header.Replace("PLACEHOLDER-DEAR", $"Chào {userName}." +
                $" Mã xác thực của bạn là: <br/>" +
                $"<h1>{code}</h1>");
            Content = Header + Footer;
        }
    }
}
