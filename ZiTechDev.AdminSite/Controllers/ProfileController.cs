using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.AdminSite.ApiClientServices.Profile;

namespace ZiTechDev.AdminSite.Controllers
{
    public class ProfileController : Controller
    {
        private IProfileApiClient _profileApiClient;
        private IHttpContextAccessor _httpContextAccessor;

        public ProfileController(
            IProfileApiClient profileApiClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _profileApiClient = profileApiClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == "UserId").Value;
        }

        // Profile
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(GetCurrentUserId());
            var result = await _profileApiClient.GetProfile(userId);
            if (result.IsSuccessed)
            {
                var model = result.ReturnedObject;
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }
    }
}
