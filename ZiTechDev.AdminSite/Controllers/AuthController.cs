using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
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

        public AuthController(
            IAuthApiClient authApiClient,
            IConfiguration configuration)
        {
            _authApiClient = authApiClient;
            _configuration = configuration;
        }

        #region Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Title = "Đăng nhập";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            // Kiểm tra tính hợp lệ của các trường thông tin đầu vào dựa trên Custom Validator / Identity Validator
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Đăng nhập";
                return View(request);
            }

            var result = await _authApiClient.Login(request);
            if (result.IsSuccessed)
            {
                // Lưu Session
                HttpContext.Session.SetString("Token", result.ReturnedObject);
                var principal = ValidateToken(result.ReturnedObject);
                var authProperties = new AuthenticationProperties()
                {
                    // Đặt hạn sử dụng của cookie phiên là 30 phút 
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                    // Xác định cookie có bị xóa khi đóng trình duyệt hay không (Có cần đăng nhập lại hay không?)
                    IsPersistent = request.RememberMe
                };
                // Lưu cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
                return RedirectToAction("Index", "Home");
            }

            // Nếu đăng nhập sai 5 lần (Tiên quyết: LockOutEnabled = true) tài khoản sẽ bị khóa đăng nhập trong 15 phút: AccessFailedCount được reset về 0, LockOutEnd được đặt lại thành thời điểm 15 phút sau
            var forgotPasswordBaseUrl = Url.ActionLink("ForgotPassword", "Auth", Request.Scheme);
            var loginWarning = await _authApiClient.LoginWarning(request.UserName, forgotPasswordBaseUrl);
            if (loginWarning.IsSuccessed)
            {
                ViewBag.Error = $" Tài khoản của bạn bị khóa tạm thời do đăng nhập sai thông tin quá nhiều." +
                $" Nếu bạn quên mật khẩu hoặc nghi ngờ tài khoản của bạn bị tấn công," +
                $" vui lòng tiến hành đổi mật khẩu. Thời gian khóa còn lại: {loginWarning.ReturnedObject} giây";
            }
            else if(int.Parse(loginWarning.Message) <= 0)
            {
                ModelState.AddModelError(string.Empty, result.ReturnedObject);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.ReturnedObject + $" ({loginWarning.Message})");
            }

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

        #region Auth/ForgotPassword
        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            HttpContext.Session.Remove("Token");
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

            var resetPasswordBaseUrl = Url.ActionLink("ResetPassword", "Auth", Request.Scheme);
            var sendMail = await _authApiClient.SendForgotPasswordEmail(request.Email, result.ReturnedObject, resetPasswordBaseUrl);
            if (sendMail.IsSuccessed)
            {
                ViewBag.Success = "Một yêu cầu xác minh danh tính đã được gửi đến " + request.Email;
            }

            ViewBag.Title = "Quên mật khẩu";
            return View(request);
        }
        #endregion

        #region Auth/UnlockOut?userId={userId}
        [HttpGet]
        public async Task<IActionResult> UnlockOut(string userId)
        {
            var result = await _authApiClient.UnlockOut(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                return View("SuccessedConfirm");
            }
            ViewBag.Error = result.Message;
            return View("FailedConfirm");
        }
        #endregion

        #region Auth/ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
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
            ModelState.AddModelError(string.Empty, result.Message);
            ViewBag.Title = "Đổi mật khẩu";
            return View(request);
        }
        #endregion

        #region Auth/VertifiedEmail?userId={userId}&token={token}
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

        #region Auth/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Token");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
        #endregion
    }
}
