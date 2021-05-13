using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.AdminSite.ApiClientServices.Auth;
using ZiTechDev.Business.Requests.Auth;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.AdminSite.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthApiClient _authApiClient;

        private readonly IConfiguration _configuration;

        public AuthController(IAuthApiClient authApiClient, IConfiguration configuration)
        {
            _authApiClient = authApiClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var token = await _authApiClient.Login(request);
            //Nếu sai thông tin đăng nhập -> token = null -> ...
            var principal = ValidateToken(token);
            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString("Token", token);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            return RedirectToAction("index", "home");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            TokenValidationParameters parameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidAudience = _configuration["Tokens:Issuer"],
                ValidIssuer = _configuration["Tokens:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]))
            };
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, parameters, out SecurityToken validatedToken);
            return principal;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("login", "auth");
        }
    }
}
