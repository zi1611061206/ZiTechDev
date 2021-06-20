using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZiTechDev.AdminSite.ApiClientServices.Profile;
using ZiTechDev.CommonModel.Requests.Profile;

namespace ZiTechDev.AdminSite.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IProfileApiClient _profileApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileController(
            IProfileApiClient profileApiClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _profileApiClient = profileApiClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(i => i.Type == "UserId").Value;
        }

        public string GetCurrentUserEmail()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
        }

        #region Profile/GetProfile
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var result = await _profileApiClient.GetProfile(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                var model = result.ReturnedObject;
                ViewBag.Title = "Hồ sơ cá nhân";

                if (TempData["Success"] != null)
                {
                    ViewBag.Success = TempData["Success"];
                }
                if (TempData["Fail"] != null)
                {
                    ViewBag.Fail = TempData["Fail"];
                }
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }
        #endregion

        #region Profile/EditProfile
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var result = await _profileApiClient.GetProfile(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                var user = result.ReturnedObject;
                var model = new ProfileEditRequest()
                {
                    Id = Guid.Parse(userId),
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    DisplayName = user.DisplayName,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender
                };
                ViewBag.Title = "Chỉnh sửa hồ sơ";
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Chỉnh sửa hồ sơ";
                return View(request);
            }

            var result = await _profileApiClient.EditProfile(request);
            if (result.IsSuccessed)
            {
                return RedirectToAction("GetProfile");
            }
            ModelState.AddModelError("", result.Message);
            ViewBag.Title = "Chỉnh sửa hồ sơ";
            return View(request);
        }
        #endregion

        #region Profile/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
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

            var result = await _profileApiClient.ChangePassword(request);
            if (result.IsSuccessed)
            {
                return RedirectToAction("Login", "Auth");
            }
            ModelState.AddModelError("", result.Message);
            ViewBag.Title = "Đổi mật khẩu";
            return View(request);
        }
        #endregion

        #region Profile/ChangeEmail
        [HttpGet]
        public IActionResult ChangeEmail()
        {
            var email = GetCurrentUserEmail();
            if (email == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var model = new ChangeEmailRequest()
            {
                OldEmail = email,
                NewEmail = email
            };
            ViewBag.Title = "Thay đổi email";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Thay đổi email";
                return View(request);
            }

            var changeEmailBaseUrl = Url.ActionLink("VertifiedChangeEmail", "Auth", Request.Scheme).Split("?")[0];
            var result = await _profileApiClient.ChangeEmail(changeEmailBaseUrl, request);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                ViewBag.Title = "Thay đổi email";
                return View(request);
            }
            ViewBag.Success = "Email chỉ được thay đổi sau khi bạn xác nhận thư được gửi đến " + request.NewEmail;
            ViewBag.Title = "Thay đổi email";
            return View(request);
        }
        #endregion

        #region Profile/Enable2FA
        [HttpGet]
        public async Task<IActionResult> Enable2FA()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _profileApiClient.Setup2FA(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                var setupCode = (JObject)JsonConvert.DeserializeObject(result.ReturnedObject);
                var model = new AuthenticateCodeRequest()
                {
                    ManualEntryKey = setupCode["manualEntryKey"].Value<string>(),
                    QrCodeSetupImageUrl = setupCode["qrCodeSetupImageUrl"].Value<string>()
                };
                ViewBag.Title = "Thiết lập 2FA";
                return View(model);
            }
            TempData["Fail"] = result.Message;
            return RedirectToAction("GetProfile", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> Enable2FA(AuthenticateCodeRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Thiết lập 2FA";
                return View(request);
            }

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _profileApiClient.Change2FA(Guid.Parse(userId), request);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                ViewBag.Title = "Thiết lập 2FA";
                return View(request);
            }
            TempData["Success"] = "Đã bật xác thực 2 bước";
            return RedirectToAction("GetProfile", "Profile");
        }
        #endregion

        #region Profile/Disable2FA
        [HttpGet]
        public IActionResult Disable2FA()
        {
            ViewBag.Title = "Thiết lập 2FA";
            return View(new AuthenticateCodeRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Disable2FA(AuthenticateCodeRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Thiết lập 2FA";
                return View(request);
            }

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _profileApiClient.Change2FA(Guid.Parse(userId), request);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                ViewBag.Title = "Thiết lập 2FA";
                return View(request);
            }
            TempData["Success"] = "Đã tắt xác thực 2 bước";
            return RedirectToAction("GetProfile", "Profile");
        }
        #endregion
    }
}
