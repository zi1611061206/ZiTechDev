using Google.Authenticator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Api.EmailConfiguration;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Engines.Email;
using ZiTechDev.CommonModel.Requests.CommonItems;
using ZiTechDev.CommonModel.Requests.Profile;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Api.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileService(
            UserManager<AppUser> userManager,
            IConfiguration configuration,
            IEmailService emailService,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region GetProfile (Handler)
        public async Task<ApiResult<ProfileViewModel>> GetProfile(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Failed<ProfileViewModel>("Không thể tìm thấy người dùng có mã: " + userId);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new ProfileViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                DateOfBirth = user.DateOfBirth,
                DateOfJoin = user.DateOfJoin,
                Gender = user.Gender,
                TwoFactorEnabled = user.TwoFactorEnabled,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                NormalizedEmail = user.NormalizedEmail,
                Email = user.Email,
                NormalizedUserName = user.NormalizedUserName,
                UserName = user.UserName,
                Roles = roles
            };
            return new Successed<ProfileViewModel>(viewModel);
        }
        #endregion

        #region EditProfile (Handler)
        public async Task<ApiResult<bool>> EditProfile(ProfileEditRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return new Failed<bool>("Không thể tìm thấy người dùng có mã: " + request.Id);
            }

            user.FirstName = request.FirstName;
            user.MiddleName = request.MiddleName;
            user.LastName = request.LastName;
            user.DisplayName = request.DisplayName;
            user.DateOfBirth = request.DateOfBirth;
            user.Gender = request.Gender;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new Failed<bool>("Lưu thất bại");
            }
            return new Successed<bool>(true);
        }
        #endregion

        #region ChangePassword (Handler)
        public async Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return new Failed<bool>("Không thể tìm thấy người dùng có mã: " + request.Id);
            }
            if (await _userManager.CheckPasswordAsync(user, request.OldPassword))
            {
                var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
                if (!result.Succeeded)
                {
                    return new Failed<bool>("Thay đổi mật khẩu thất bại");
                }
                return new Successed<bool>(true);
            }
            else
            {
                return new Failed<bool>("Mật khẩu hiện tại không đúng");
            }
        }
        #endregion

        #region ChangeEmail (Handler)
        public async Task<ApiResult<bool>> ChangeEmail(string changeEmailBaseUrl, ChangeEmailRequest request)
        {
            var isExisted = await _userManager.FindByEmailAsync(request.NewEmail) != null;
            if (isExisted)
            {
                return new Failed<bool>("Email này đã được đăng ký bởi người dùng khác");
            }
            var user = await _userManager.FindByEmailAsync(request.OldEmail);
            if (user == null)
            {
                return new Failed<bool>("Không thể tìm thấy người dùng");
            }
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var changeEmailUrl = changeEmailBaseUrl + $"?userId={user.Id}&token={encodedToken}&newEmail={request.NewEmail}";

            var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
            template.ChangeEmailConfirmation(changeEmailUrl, user.UserName);

            var email = new EmailItem();
            email.Senders.Add(new EmailBase(_configuration.GetValue<string>("EmailSender:Name"), _configuration.GetValue<string>("EmailSender:Address")));
            email.Receivers.Add(new EmailBase(user.UserName, request.NewEmail));
            email.Subject = template.Subject;
            email.Body = template.Content;
            if (await _emailService.SendAsync(email))
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>("Lỗi hệ thống: không thể gửi xác thực tới " + request.NewEmail);
        }
        #endregion

        #region Setup2FA (Handler)
        public async Task<ApiResult<SetupCode>> Setup2FA(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Failed<SetupCode>("Không tìm thấy người dùng");
            }
            TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
            var setupCode = twoFactorAuthenticator.GenerateSetupCode(
                _configuration.GetValue<string>("Token:Issuer"), 
                user.UserName, 
                $"{_configuration.GetValue<string>("Token:Key")}+{user.UserName}", 
                false, 3);
            return new Successed<SetupCode>(setupCode);
        }
        #endregion

        #region Change2FA (Handler)
        public async Task<ApiResult<bool>> Change2FA(Guid userId, AuthenticateCodeRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Failed<bool>("Không tìm thấy người dùng");
            }
            TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator(); 
            bool isValid = twoFactorAuthenticator.ValidateTwoFactorPIN(
                $"{_configuration.GetValue<string>("Token:Key")}+{user.UserName}", 
                request.PinCode);
            if (isValid)
            {
                user.TwoFactorEnabled = !user.TwoFactorEnabled;
                await _userManager.UpdateAsync(user);
                return new Successed<bool>(true);
            }
            return new Failed<bool>("Mã xác thực không hợp lệ");
        }
        #endregion
    }
}
