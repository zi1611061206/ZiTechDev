using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.AdminSite.ApiClientServices.User;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.AdminSite.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;

        private readonly IConfiguration _configuration;

        public UserController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session.GetString("Token");
            var filter = new UserFilter()
            {
                BearerToken = session
            };
            var data = await _userApiClient.Get(filter);
            ViewData["Users"] = data;
            ViewData["Filter"] = filter;
            var cb = new GenderSelector();
            cb.Items = new List<GenderItem>() {
                new GenderItem(-1, "Tất cả"),
                new GenderItem(0, "Nam"),
                new GenderItem(1, "Nữ"),
                new GenderItem(2, "Giới tính khác")
            };
            ViewData["CbGender"] = cb;
            return View();
        }

        [HttpPost("")]
        public async Task<IActionResult> Index(UserFilter filter)
        {
            var session = HttpContext.Session.GetString("Token");
            filter.BearerToken = session;
            var data = await _userApiClient.Get(filter);
            ViewData["Users"] = data;
            ViewData["Filter"] = filter;
            var cb = new GenderSelector();
            cb.Items = new List<GenderItem>() {
                new GenderItem(-1, "Tất cả"),
                new GenderItem(0, "Nam"),
                new GenderItem(1, "Nữ"),
                new GenderItem(2, "Giới tính khác")
            };
            ViewData["CbGender"] = cb;
            return View();
        }
    }
}
