using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
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
using ZiTechDev.AdminSite.EmailConfiguration;
using ZiTechDev.CommonModel.Engines.Email;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.AdminSite.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthApiClient _authApiClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthController(
            IAuthApiClient authApiClient,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,
            IWebHostEnvironment webHostEnvironment)
        {
            _authApiClient = authApiClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(i => i.Type == "UserId").Value;
        }

        #region Login
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
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", result.Message);
            ViewBag.Title = "Đăng nhập";
            return View(request);
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
        #endregion

        #region Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "Auth");
        }
        #endregion

        #region Read Profile
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var result = await _authApiClient.GetProfile(Guid.Parse(GetCurrentUserId()));
            if (result.IsSuccessed)
            {
                var model = result.ReturnedObject;
                ViewBag.Title = "Hồ sơ cá nhân";
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }
        #endregion

        #region EditProfile
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
        #endregion

        #region Change Password
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
        #endregion

        #region Vertified Email
        [HttpGet]
        public async Task<IActionResult> VertifiedEmail(string userId, string token)
        {
            var result = await _authApiClient.VertifiedEmail(Guid.Parse(userId), token);
            if (result.IsSuccessed)
            {
                return View("SuccessedConfirm");
            }
            ViewBag.Error = result.Message;
            return View("FailedConfirm");
        }
        #endregion

        #region Forgot Password
        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.Title = "Quên mật khẩu";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Quên mật khẩu";
                return View(request);
            }

            var result = await _authApiClient.ForgotPassword(request);

            if (!result.IsSuccessed)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                ViewBag.Title = "Quên mật khẩu";
                return View(request);
            }

            var user = await _authApiClient.GetByEmail(request.Email);
            var url = Url.ActionLink("ResetPassword", "Auth", new { userId = user.ReturnedObject.Id, token = result.ReturnedObject }, Request.Scheme);
            var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
            template.ForgotPassword(url, user.ReturnedObject.UserName);

            var email = new EmailItem();
            email.Senders.Add(new EmailBase(_configuration.GetValue<string>("EmailSender:Name"), _configuration.GetValue<string>("EmailSender:Address")));
            email.Receivers.Add(new EmailBase(user.ReturnedObject.UserName, user.ReturnedObject.Email));
            email.Subject = template.Subject;
            email.Body = template.Content;
            await _emailService.SendAsync(email);

            ModelState.Clear();
            ViewBag.Success = "Một yêu cầu xác minh danh tính đã được gửi đến " + request.Email; 
            ViewBag.Title = "Quên mật khẩu";
            return View(request);
        }
        #endregion

        #region Reset Password
        [HttpGet]
        public ActionResult ResetPassword(string userId, string token)
        {
            var model = new ResetPasswordRequest()
            {
                Id = Guid.Parse(userId),
                Token = token
            };
            ViewBag.Title = "Đổi mật khẩu";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Đổi mật khẩu";
                return View(request);
            }

            var result = await _authApiClient.ResetPassword(request);
            if (result.IsSuccessed)
            {
                return View("SuccessedConfirm");
            }

            ModelState.AddModelError("", result.Message);
            ViewBag.Title = "Đổi mật khẩu";
            return View(request);
        }
        #endregion
    }
}
