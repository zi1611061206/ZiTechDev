using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
        private readonly IConfiguration _config;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<ApiResult<string>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new Failed<string>("Người dùng không tồn tại");
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
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
    }
}
