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
using ZiTechDev.CommonModel.Requests.User;
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
        public async Task<ApiResult<string>> Register(RegisterRequest request)
        {
            if (await _userManager.FindByNameAsync(request.UserName) != null)
            {
                return new Failed<string>("Tên đăng nhập đã tồn tại");
            }

            var user = new AppUser()
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                DisplayName = request.DisplayName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                UserName = request.UserName
            };

            if (await _userManager.FindByEmailAsync(request.Email) != null && _userManager.IsEmailConfirmedAsync(user).Result)
            {
                return new Failed<string>("Địa chỉ email đã được đăng ký và xác thực");
            }

            var result = await _userManager.CreateAsync(user, new PasswordGenerator().Generate());
            if (!result.Succeeded)
            {
                return new Failed<string>("Đăng ký thất bại");
            }
            return new Successed<string>(user.Id.ToString());
        }
        #endregion

        #region Login (Handler)
        public async Task<ApiResult<string>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new Failed<string>("Người dùng không tồn tại");
            }
            if (!user.EmailConfirmed)
            {
                return new Failed<string>("Tài khoản chưa được kích hoạt. Vui lòng kiểm tra hộp thư của bạn");
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return new Failed<string>("Mật khẩu đăng nhập không đúng");
            }

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
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
                );

            return new Successed<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }
        #endregion

        #region LoginWarning (Handler)
        public async Task<ApiResult<int>> LoginWarning(string userName, string forgotPasswordBaseUrl)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new Failed<int>("0");
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                if (user.EmailConfirmed)
                {
                    var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
                    template.LoginWarning(forgotPasswordBaseUrl, user.UserName);

                    var email = new EmailItem();
                    email.Senders.Add(new EmailBase(_configuration.GetValue<string>("EmailSender:Name"), _configuration.GetValue<string>("EmailSender:Address")));
                    email.Receivers.Add(new EmailBase(user.UserName, user.Email));
                    email.Subject = template.Subject;
                    email.Body = template.Content;
                    await _emailService.SendAsync(email);
                }
                var timeSpan = (int)Math.Floor(user.LockoutEnd.Value.UtcDateTime.Subtract(DateTime.UtcNow).TotalSeconds);
                return new Successed<int>(timeSpan);
            }
            return new Failed<int>(user.AccessFailedCount.ToString());
        }
        #endregion

        #region ForgotPassword (Handler)
        public async Task<ApiResult<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new Failed<string>("Người dùng không tồn tại");
            }
            if (!user.EmailConfirmed)
            {
                return new Failed<string>("Email người dùng chưa được xác minh");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            return new Successed<string>(encodedToken);
        }
        #endregion

        #region SendForgotPasswordEmail (Handler)
        public async Task<ApiResult<bool>> SendForgotPasswordEmail(string emailAddress, string token, string resetPasswordBaseUrl)
        {
            var user = await _userManager.FindByEmailAsync(emailAddress);
            var resetPasswordUrl = resetPasswordBaseUrl + $"?userId={user.Id}&token={token}";

            var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
            template.ForgotPassword(resetPasswordUrl, user.UserName);

            var email = new EmailItem();
            email.Senders.Add(new EmailBase(_configuration.GetValue<string>("EmailSender:Name"), _configuration.GetValue<string>("EmailSender:Address")));
            email.Receivers.Add(new EmailBase(user.UserName, user.Email));
            email.Subject = template.Subject;
            email.Body = template.Content;
            if(await _emailService.SendAsync(email))
            {
                return new Successed<bool>(true);
            }

            return new Failed<bool>(string.Empty);
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
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            return new Successed<bool>(true);
        }
        #endregion

        #region VertifiedEmail (Handler)
        public async Task<ApiResult<bool>> VertifiedEmail(Guid userId, string token)
        {
            if (string.IsNullOrEmpty(userId.ToString()) || string.IsNullOrEmpty(token))
            {
                return new Failed<bool>("Thông tin không hợp lệ");
            }
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
    }
}
