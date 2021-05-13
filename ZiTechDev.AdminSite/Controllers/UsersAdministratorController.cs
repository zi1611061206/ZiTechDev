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
    public class UsersAdministratorController : BaseController
    {
        private readonly IUserApiClient _userApiClient;

        public UsersAdministratorController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session.GetString("Token");
            var filter = new UserFilter()
            {
                BearerToken = session
            };
            var data = await _userApiClient.Get(filter);
            ViewBag.Title = "Danh sách thành viên";
            ViewData["Users"] = data;
            ViewData["Filter"] = filter;
            var cb = new GenderSelector
            {
                Items = new List<GenderItem>()
                {
                    new GenderItem(-1, "Tất cả"),
                    new GenderItem(0, "Nam"),
                    new GenderItem(1, "Nữ"),
                    new GenderItem(2, "Giới tính khác")
                }
            };
            ViewData["CbGender"] = cb;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserFilter filter)
        {
            var session = HttpContext.Session.GetString("Token");
            filter.BearerToken = session;
            var data = await _userApiClient.Get(filter);
            ViewBag.Title = "Danh sách thành viên";
            ViewData["Users"] = data;
            ViewData["Filter"] = filter;
            var cb = new GenderSelector
            {
                Items = new List<GenderItem>()
                {
                    new GenderItem(-1, "Tất cả"),
                    new GenderItem(0, "Nam"),
                    new GenderItem(1, "Nữ"),
                    new GenderItem(2, "Giới tính khác")
                }
            };
            ViewData["CbGender"] = cb;
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateRequest request)
        {
            var session = HttpContext.Session.GetString("Token");
            request.BearerToken = session;
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _userApiClient.Create(request);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View(request);
        }
    }
}
