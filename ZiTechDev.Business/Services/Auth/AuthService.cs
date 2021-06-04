using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Requests.Auth;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Business.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

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

        public async Task<ApiResult<string>> Register(RegisterRequest request)
        {
            if(await _userManager.FindByNameAsync(request.UserName) != null)
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

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new Failed<string>("Đăng ký thất bại");
            }
            return new Successed<string>(user.Id.ToString());
        }

        public async Task<ApiResult<bool>> EditProfile(EditProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return new Failed<bool>("Không thể tìm thấy người dùng có mã: " + request.Id);
            }
            if (await _userManager.Users.AnyAsync(x => x.Email.Equals(request.Email) && x.Id != request.Id))
            {
                return new Failed<bool>("Địa chỉ email đã được đăng ký bởi người dùng khác");
            }

            user.FirstName = request.FirstName;
            user.MiddleName = request.MiddleName;
            user.LastName = request.LastName;
            user.DisplayName = request.DisplayName;
            user.PhoneNumber = request.PhoneNumber;
            user.Email = request.Email;
            user.DateOfBirth = request.DateOfBirth;
            user.Gender = request.Gender;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new Failed<bool>("Lưu thất bại");
            }
            return new Successed<bool>(true);
        }

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

        public async Task<ApiResult<bool>> ConfirmEmail(string userName, string token)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
            {
                return new Failed<bool>("Thông tin không hợp lệ");
            }
            var user = await _userManager.FindByNameAsync(userName);
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
            {
                return new Failed<bool>("Vượt quá thời gian xác nhận. Vui lòng đăng ký lại hoặc liên hệ quản trị viên");
            }
            return new Successed<bool>(true);
        } 
    }
}
