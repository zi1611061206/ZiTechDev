using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.AdminSite.ApiClientServices.Auth;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.AdminSite.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthApiClient _authApiClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(
            IAuthApiClient authApiClient,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _authApiClient = authApiClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.Title = "Đăng nhập";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Đăng nhập";
                return View(request);
            }

            var result = await _authApiClient.Login(request);

            if (result.IsSuccessed)
            {
                var principal = ValidateToken(result.ReturnedObject);
                var authProperties = new AuthenticationProperties()
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    IsPersistent = false
                };
                HttpContext.Session.SetString("Token", result.ReturnedObject);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
                return RedirectToAction("index", "home");
            }
            ModelState.AddModelError("", result.Message);
            ViewBag.Title = "Đăng nhập";
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("login", "auth");
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

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            if (GetCurrentUserId() == null)
            {
                RedirectToAction("Login", "Auth");
            }

            var userId = Guid.Parse(GetCurrentUserId());
            var result = await _authApiClient.GetProfile(userId);
            if (result.IsSuccessed)
            {
                var model = result.ReturnedObject;
                ViewBag.Title = "Hồ sơ cá nhân";
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }

        public string GetCurrentUserId()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            if (currentUser != null)
            {
                return currentUser.Claims.First(i => i.Type == "UserId").Value;
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string userId)
        {
            var result = await _authApiClient.GetProfile(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                var user = result.ReturnedObject;
                var model = new EditProfileRequest()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    DisplayName = user.DisplayName,
                    DateOfBirth = user.DateOfBirth,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Gender = user.Gender
                };
                ViewBag.Title = "Chỉnh sửa hồ sơ";
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Chỉnh sửa hồ sơ";
                return View(request);
            }

            var result = await _authApiClient.EditProfile(request);
            if (result.IsSuccessed)
            {
                return RedirectToAction("GetProfile");
            }
            ModelState.AddModelError("", result.Message);
            ViewBag.Title = "Chỉnh sửa hồ sơ";
            return View(request);
        }

        [HttpGet]
        public IActionResult ChangePassword(string userId)
        {
            var model = new ChangePasswordRequest()
            {
                Id = Guid.Parse(userId)
            };
            ViewBag.Title = "Đổi mật khẩu";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Đổi mật khẩu";
                return View(request);
            }

            var result = await _authApiClient.ChangePassword(request);
            if (result.IsSuccessed)
            {
                return RedirectToAction("Login", "Auth");
            }
            ModelState.AddModelError("", result.Message);
            ViewBag.Title = "Đổi mật khẩu";
            return View(request);
        }
    }
}
