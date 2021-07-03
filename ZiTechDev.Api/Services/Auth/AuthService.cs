using Google.Authenticator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Api.EmailConfiguration;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Engines.Email;
using ZiTechDev.CommonModel.Requests.Auth;
using ZiTechDev.CommonModel.Requests.CommonItems;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            IEmailService emailService,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Register (Handler)
        public async Task<ApiResult<bool>> Register(string activatedEmailBaseUrl, RegisterRequest request)
        {
            if (await _userManager.FindByNameAsync(request.UserName) != null)
            {
                return new Failed<bool>("Tên đăng nhập đã tồn tại");
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    return new Failed<bool>("Địa chỉ email đã được đăng ký");
                }
                else
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            user = new AppUser()
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                DisplayName = string.IsNullOrEmpty(request.DisplayName) ? request.UserName : request.DisplayName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                UserName = request.UserName
            };

            var result = await _userManager.CreateAsync(user, new PasswordGenerator().Generate());
            if (!result.Succeeded)
            {
                return new Failed<bool>("Đăng ký thất bại");
            }
            await _userManager.AddToRoleAsync(user, "User");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var activatedEmailUrl = activatedEmailBaseUrl + $"?userId={user.Id}&token={encodedToken}";

            var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
            template.EmailConfirmation(activatedEmailUrl, user.UserName);

            var email = new EmailItem();
            email.Senders.Add(new EmailBase(_configuration.GetValue<string>("EmailSender:Name"), _configuration.GetValue<string>("EmailSender:Address")));
            email.Receivers.Add(new EmailBase(user.UserName, user.Email));
            email.Subject = template.Subject;
            email.Body = template.Content;
            if (await _emailService.SendAsync(email))
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>("Lỗi hệ thống: không thể gửi xác thực đến " + user.Email);
        }
        #endregion

        #region ValidateUserName (Handler)
        public async Task<ApiResult<bool>> ValidateUserName(LoginUserNameRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new Failed<bool>("Người dùng không tồn tại");
            }
            else if (!user.EmailConfirmed)
            {
                return new Failed<bool>("Tài khoản chưa được kích hoạt. Vui lòng kiểm tra hộp thư của bạn");
            }
            return new Successed<bool>(true);
        }
        #endregion

        #region ValidateLogin (Handler)
        public async Task<ApiResult<bool>> ValidateLogin(string resetPasswordBaseUrl, LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (!(await _userManager.CheckPasswordAsync(user, request.Password)))
            {
                if (await _userManager.IsLockedOutAsync(user))
                {
                    var resetPasswordtoken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetPasswordtoken));
                    var resetPasswordUrl = resetPasswordBaseUrl + $"?userId={user.Id}&token={encodedToken}";

                    var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
                    template.LoginWarning(resetPasswordUrl, user.UserName);

                    var email = new EmailItem();
                    email.Senders.Add(new EmailBase(_configuration.GetValue<string>("EmailSender:Name"), _configuration.GetValue<string>("EmailSender:Address")));
                    email.Receivers.Add(new EmailBase(user.UserName, user.Email));
                    email.Subject = template.Subject;
                    email.Body = template.Content;
                    await _emailService.SendAsync(email);

                    var timeSpan = (int)Math.Ceiling(user.LockoutEnd.Value.UtcDateTime.Subtract(DateTime.UtcNow).TotalSeconds / 60);
                    return new Failed<bool>($" Tài khoản của bạn bị khóa tạm thời do đăng nhập sai thông tin quá nhiều." +
                        $" Nếu bạn quên mật khẩu hoặc nghi ngờ tài khoản của bạn bị tấn công," +
                        $" vui lòng tiến hành đổi mật khẩu. Bạn có thể thử đăng nhập sau: ~{timeSpan} phút");
                }
                return new Failed<bool>("Mật khẩu đăng nhập không đúng " + $"({user.AccessFailedCount})");
            }
            else
            {
                return new Successed<bool>(user.TwoFactorEnabled);
            }
        }
        #endregion

        #region Login (Handler)
        public async Task<ApiResult<string>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new Failed<string>("Người dùng không tồn tại hoặc đã bị xóa");
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (result.Succeeded)
            {
                var roles = _userManager.GetRolesAsync(user);
                var claims = new[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                    new Claim(ClaimTypes.Name, user.DisplayName),
                    new Claim("UserName", user.UserName),
                    new Claim(ClaimTypes.Role, string.Join(";", roles))
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Tokens:Issuer"], _configuration["Tokens:Issuer"],
                    claims, expires: DateTime.Now.AddHours(3), signingCredentials: creds);
                return new Successed<string>(new JwtSecurityTokenHandler().WriteToken(token));
            }
            return new Failed<string>("Đăng nhập không thành công");
        }
        #endregion

        #region SendToAuthenticator (Handler)
        public async Task<ApiResult<bool>> SendToAuthenticator(string userName, string provider)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new Failed<bool>("Người dùng không tồn tại hoặc đã bị xóa");
            }
            switch (provider.ToLower())
            {
                case "google":
                    return new Successed<bool>(true);
                case "microsoft":
                    return new Successed<bool>(true);
                case "sms":
                    return new Successed<bool>(true);
                case "recovery":
                    return new Successed<bool>(true);
                case "email":
                    var code = await _userManager.GenerateTwoFactorTokenAsync(user, provider);
                    var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
                    template.Authenticate2FA(userName, code);
                    var email = new EmailItem();
                    email.Senders.Add(new EmailBase(_configuration.GetValue<string>("EmailSender:Name"), _configuration.GetValue<string>("EmailSender:Address")));
                    email.Receivers.Add(new EmailBase(user.UserName, user.Email));
                    email.Subject = template.Subject;
                    email.Body = template.Content;
                    if (await _emailService.SendAsync(email))
                    {
                        return new Successed<bool>(true);
                    }
                    return new Failed<bool>("Không thể gửi mã đến " + user.Email);
                default:
                    return new Failed<bool>("Phương thức xác thực không hợp lệ");
            }
        }
        #endregion

        #region Authenticate2FA (Handler)
        public async Task<ApiResult<string>> Authenticate2FA(Authenticate2FARequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new Failed<string>("Người dùng không tồn tại hoặc đã bị xóa");
            }

            bool isValid;
            switch (request.Provider.ToLower())
            {
                case "google":
                    TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
                    isValid = twoFactorAuthenticator.ValidateTwoFactorPIN(
                        $"{_configuration.GetValue<string>("Token:Key")}+{user.UserName}",
                        request.PinCode);
                    break;
                case "microsoft":
                    return new Failed<string>("Phương thức xác thực không khả dụng");
                case "sms":
                    return new Failed<string>("Phương thức xác thực không khả dụng");
                case "recovery":
                    var resultRecoveryAuth = await _signInManager.TwoFactorRecoveryCodeSignInAsync(request.PinCode);
                    isValid = resultRecoveryAuth.Succeeded;
                    break;
                case "email":
                    var resultEmailAuth = await _signInManager.TwoFactorSignInAsync(request.Provider, request.PinCode, request.IsPersistent, request.IsRememberClient);
                    isValid = resultEmailAuth.Succeeded;
                    break;
                default:
                    return new Failed<string>("Phương thức xác thực không hợp lệ");
            }

            if (isValid)
            {
                return new Failed<string>("Mã xác thực không hợp lệ");
            }
            else
            {
                var roles = _userManager.GetRolesAsync(user);
                var claims = new[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                    new Claim(ClaimTypes.Name, user.DisplayName),
                    new Claim("UserName", user.UserName),
                    new Claim(ClaimTypes.Role, string.Join(";", roles))
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Tokens:Issuer"], _configuration["Tokens:Issuer"],
                    claims, expires: DateTime.Now.AddHours(3), signingCredentials: creds);
                return new Successed<string>(new JwtSecurityTokenHandler().WriteToken(token));
            }
        }
        #endregion

        #region ForgotPassword (Handler)
        public async Task<ApiResult<bool>> ForgotPassword(string resetPasswordBaseUrl, ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new Failed<bool>("Người dùng không tồn tại");
            }
            if (!user.EmailConfirmed)
            {
                return new Failed<bool>("Email người dùng chưa được xác minh");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var resetPasswordUrl = resetPasswordBaseUrl + $"?userId={user.Id}&token={encodedToken}";

            var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
            template.ForgotPassword(resetPasswordUrl, user.UserName);

            var email = new EmailItem();
            email.Senders.Add(new EmailBase(_configuration.GetValue<string>("EmailSender:Name"), _configuration.GetValue<string>("EmailSender:Address")));
            email.Receivers.Add(new EmailBase(user.UserName, user.Email));
            email.Subject = template.Subject;
            email.Body = template.Content;
            if (await _emailService.SendAsync(email))
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>("Lỗi hệ thông: không thể gửi xác thực");
        }
        #endregion

        #region UnlockOut (Handler)
        public async Task<ApiResult<bool>> UnlockOut(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Failed<bool>("Người dùng không tồn tại hoặc đã bị xóa");
            }
            if (!(await _userManager.IsLockedOutAsync(user)))
            {
                return new Failed<bool>("Người dùng không không bị khóa");
            }
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
            if (!result.Succeeded)
            {
                return new Failed<bool>("Mở khóa thất bại");
            }
            return new Successed<bool>(true);
        }
        #endregion

        #region ResetPassword (Handler)
        public async Task<ApiResult<bool>> ResetPassword(ResetPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Id.ToString()) || string.IsNullOrEmpty(request.Token))
            {
                return new Failed<bool>("Thông tin xác minh không hợp lệ");
            }
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return new Failed<bool>("Người dùng không tồn tại hoặc đã bị xóa");
            }
            string decodedToken;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            }
            catch
            {
                return new Failed<bool>("Token không hợp lệ");
            }
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.Password);
            if (!result.Succeeded)
            {
                return new Failed<bool>("Đặt lại mật khẩu không thành công do token đã thay đổi.");
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
            }
            return new Successed<bool>(true);
        }
        #endregion

        #region VertifiedEmail (Handler)
        public async Task<ApiResult<bool>> VertifiedEmail(Guid userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            string decodedToken;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            }
            catch
            {
                return new Failed<bool>("Token không hợp lệ");
            }
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
            {
                return new Failed<bool>("Xác minh email không thành công do token đã thay đổi.");
            }
            return new Successed<bool>(true);
        }
        #endregion

        #region VertifiedChangeEmail (Handler)
        public async Task<ApiResult<bool>> VertifiedChangeEmail(Guid userId, string token, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Failed<bool>("Không thể tìm thấy người dùng");
            }
            string decodedToken;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            }
            catch
            {
                return new Failed<bool>("Token không hợp lệ");
            }
            var result = await _userManager.ChangeEmailAsync(user, newEmail, decodedToken);
            if (!result.Succeeded)
            {
                return new Failed<bool>("Xác minh email không thành công do token đã thay đổi.");
            }
            return new Successed<bool>(true);
        }
        #endregion

        #region ActivatedEmail (Handler)
        public async Task<ApiResult<string>> ActivatedEmail(Guid userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            string decodedToken;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            }
            catch
            {
                return new Failed<string>("Token không hợp lệ");
            }
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
            {
                return new Failed<string>("Kích hoạt không thành công do token đã thay đổi.");
            }
            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetPasswordToken));
            return new Successed<string>(encodedToken);
        }
        #endregion
    }
}
