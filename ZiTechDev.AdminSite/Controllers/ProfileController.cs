using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Profile/GetProfile
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var result = await _profileApiClient.GetProfile(Guid.Parse(GetCurrentUserId()));
            if (result.IsSuccessed)
            {
                var model = result.ReturnedObject;
                ViewBag.Title = "Hồ sơ cá nhân";
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }
        #endregion

        #region Profile/EditProfile?userId={userId}
        [HttpGet]
        public async Task<IActionResult> EditProfile(string userId)
        {
            var result = await _profileApiClient.GetProfile(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                var user = result.ReturnedObject;
                var model = new ProfileEditRequest()
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

        #region Profile/ChangePassword?userId={userId}
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
    }
}
