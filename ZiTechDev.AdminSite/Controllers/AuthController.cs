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

        #region Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterRequest();
            ViewBag.Title = "Đăng ký";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Đăng ký";
                return View(request);
            }

            var activatedEmailBaseUrl = Url.ActionLink("ActivatedEmail", "Auth", Request.Scheme).Split("?")[0];
            var result = await _authApiClient.Register(activatedEmailBaseUrl, request);
            if (result.IsSuccessed)
            {
                ViewBag.Success = "Một yêu cầu kích hoạt đã được gửi đến " + request.Email;
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
            }
            ViewBag.Title = "Đăng ký";
            return View(request);
        }
        #endregion

        #region Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return RedirectToAction("LoginUserName", new { userNameOrEmail = "" });
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
            var resetPasswordBaseUrl = Url.ActionLink("ResetPassword", "Auth", Request.Scheme).Split("?")[0];
            var checker = await _authApiClient.ValidateLogin(resetPasswordBaseUrl, request);
            if (!checker.IsSuccessed)
            {
                ModelState.AddModelError(string.Empty, checker.Message);
                ViewBag.Title = "Đăng nhập";
                return View(request);
            }
            var twoFactorsEnabled = checker.ReturnedObject;
            if (twoFactorsEnabled)
            {
                return RedirectToAction("GetAuthenticationMethod", "Auth", new { userNameOrEmail = request.UserNameOrEmail, rememberMe = request.RememberMe });
            }
            else
            {
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
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    ViewBag.Title = "Đăng nhập";
                    return View(request);
                }
            }
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

        #region Auth/LoginUserName?userNameOrEmail={userNameOrEmail}
        [HttpGet]
        public IActionResult LoginUserName(string userNameOrEmail)
        {
            var model = new LoginUserNameRequest();
            if (!string.IsNullOrEmpty(userNameOrEmail))
            {
                model.UserNameOrEmail = userNameOrEmail;
            }
            ViewBag.Title = "Đăng nhập";
            return View("LoginUserName", model);
        }

        [HttpPost]
        public async Task<IActionResult> LoginUserName(LoginUserNameRequest request)
        {
            // Kiểm tra tính hợp lệ của các trường thông tin đầu vào dựa trên Custom Validator / Identity Validator
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Đăng nhập";
                return View(request);
            }
            var checker = await _authApiClient.ValidateUserName(request);
            if (!checker.IsSuccessed)
            {
                ModelState.AddModelError(string.Empty, checker.Message);
                ViewBag.Title = "Đăng nhập";
                return View(request);
            }
            else if (!checker.ReturnedObject) // Email chưa được kích hoạt
            {
                return RedirectToAction("ActiveAccount", new { userNameOrEmail = request.UserNameOrEmail });
            }
            else
            {
                var model = new LoginRequest()
                {
                    UserNameOrEmail = request.UserNameOrEmail
                };
                ViewBag.Title = "Đăng nhập";
                return View("Login", model);
            }
        }
        #endregion

        #region Auth/ActiveAccount?userNameOrEmail={userNameOrEmail}
        [HttpGet]
        public IActionResult ActiveAccount(string userNameOrEmail)
        {
            var model = new LoginUserNameRequest()
            {
                UserNameOrEmail = userNameOrEmail
            };

            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }
            if (TempData["Fail"] != null)
            {
                ViewBag.Fail = TempData["Fail"];
            }
            ViewBag.UserNameOrEmail = userNameOrEmail;
            ViewBag.Title = "Yêu cầu kích hoạt";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ActiveAccount(LoginUserNameRequest request)
        {
            var activatedEmailBaseUrl = Url.ActionLink("ActivatedEmail", "Auth", Request.Scheme).Split("?")[0];
            var result = await _authApiClient.ActiveAccount(activatedEmailBaseUrl, request.UserNameOrEmail);
            if (!result.IsSuccessed)
            {
                TempData["Fail"] = result.Message;
            }
            else
            {
                TempData["Success"] = "Vui lòng kiểm tra hộp thư của bạn!";
            }
            return RedirectToAction("ActiveAccount", new { request.UserNameOrEmail });
        }
        #endregion

        #region Auth/GetAuthenticationMethod?userNameOrEmail={userNameOrEmail}&rememberMe={rememberMe}
        [HttpGet]
        public IActionResult GetAuthenticationMethod(string userNameOrEmail, bool rememberMe)
        {
            var model = new Authenticate2FARequest()
            {
                UserNameOrEmail = userNameOrEmail,
                RememberMe = rememberMe
            };
            ViewBag.Title = "Phương thức xác thực";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetAuthenticationMethod(Authenticate2FARequest request)
        {
            var result = await _authApiClient.GetAuthenticationMethod(request.UserNameOrEmail, request.Provider);
            if (result.IsSuccessed)
            {
                return RedirectToAction("Authenticate2FA", new { userNameOrEmail = request.UserNameOrEmail, rememberMe = request.RememberMe, provider = request.Provider });
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View(request);
        }
        #endregion

        #region Auth/Authenticate2FA?userNameOrEmail={userNameOrEmail}&rememberMe={rememberMe}&provider={provider}
        [HttpGet]
        public IActionResult Authenticate2FA(string userNameOrEmail, bool rememberMe, string provider)
        {
            var model = new Authenticate2FARequest()
            {
                UserNameOrEmail = userNameOrEmail,
                RememberMe = rememberMe,
                Provider = provider
            };
            ViewBag.Title = "Xác thực bước 2";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate2FA(Authenticate2FARequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Xác thực bước 2";
                return View(request);
            }
            var result = await _authApiClient.Authenticate2FA(request);
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
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                ViewBag.Title = "Xác thực bước 2";
                return View(request);
            }
        }
        #endregion

        #region Auth/GetForgotPasswordMethod?userNameOrEmail={userNameOrEmail}
        [HttpGet]
        public IActionResult GetForgotPasswordMethod(string userNameOrEmail)
        {
            var model = new AuthenticateForgotPasswordRequest()
            {
                UserNameOrEmail = userNameOrEmail
            };
            ViewBag.Title = "Phương thức xác thực";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetForgotPasswordMethod(AuthenticateForgotPasswordRequest request)
        {
            var result = await _authApiClient.GetAuthenticationMethod(request.UserNameOrEmail, request.Provider);
            if (result.IsSuccessed)
            {
                return RedirectToAction("AuthenticateForgotPassword", new { userNameOrEmail = request.UserNameOrEmail, provider = request.Provider });
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View(request);
        }
        #endregion

        #region Auth/AuthenticateForgotPassword?userNameOrEmail={userNameOrEmail}&provider={provider}
        [HttpGet]
        public IActionResult AuthenticateForgotPassword(string userNameOrEmail, string provider)
        {
            var model = new AuthenticateForgotPasswordRequest()
            {
                UserNameOrEmail = userNameOrEmail,
                Provider = provider
            };
            ViewBag.Title = "Xác thực danh tính";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateForgotPassword(AuthenticateForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Xác thực danh tính";
                return View(request);
            }
            var result = await _authApiClient.AuthenticateForgotPassword(request);
            if (result.IsSuccessed)
            {
                return RedirectToAction("ResetPassword", "Auth", new { userNameOrEmail = request.UserNameOrEmail, token = result.ReturnedObject });
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                ViewBag.Title = "Xác thực danh tính";
                return View(request);
            }
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

        #region Auth/ResetPassword?userNameOrEmail={userNameOrEmail}&token={token}
        [HttpGet]
        public IActionResult ResetPassword(string userNameOrEmail, string token)
        {
            var model = new ResetPasswordRequest()
            {
                UserNameOrEmail = userNameOrEmail,
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

        #region Auth/VertifiedChangeEmail?userId={userId}&token={token}&newEmail={newEmail}
        [HttpGet]
        public async Task<IActionResult> VertifiedChangeEmail(string userId, string token, string newEmail)
        {
            var result = await _authApiClient.VertifiedChangeEmail(Guid.Parse(userId), token, newEmail);
            if (result.IsSuccessed)
            {
                return View("SuccessedConfirm");
            }
            ViewBag.Error = result.Message;
            return View("FailedConfirm");
        }
        #endregion

        #region Auth/ActivatedEmail?userNameOrEmail={userNameOrEmail}&token={token}
        [HttpGet]
        public async Task<IActionResult> ActivatedEmail(string userNameOrEmail, string token)
        {
            var result = await _authApiClient.ActivatedEmail(userNameOrEmail, token);
            if (result.IsSuccessed)
            {
                return RedirectToAction("ResetPassword", "Auth", new { userNameOrEmail, token = result.ReturnedObject });
            }
            ViewBag.Error = result.Message;
            return View("FailedConfirm");
        }
        #endregion

        #region Auth/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Token");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
        #endregion
    }
}
