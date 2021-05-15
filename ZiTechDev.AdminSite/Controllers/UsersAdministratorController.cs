using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.AdminSite.ApiClientServices.User;
using ZiTechDev.Business.Engines.Paginition;
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

        // Read
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var filter = new UserFilter();
            var result = await _userApiClient.Get(filter);
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

            ViewBag.Title = "Danh sách thành viên";
            ViewData["Users"] = result.ReturnedObject;
            ViewData["Filter"] = filter;
            ViewData["CbGender"] = cb;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserFilter filter)
        {
            var result = await _userApiClient.Get(filter);
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

            ViewBag.Title = "Danh sách thành viên";
            ViewData["Users"] = result.ReturnedObject;
            ViewData["Filter"] = filter;
            ViewData["CbGender"] = cb;

            return View();
        }

        // Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userApiClient.Create(request);

            if (result.IsSuccessed)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        // Update
        [HttpGet]
        public async Task<IActionResult> Update(string userId)
        {
            var result = await _userApiClient.GetById(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                var user = result.ReturnedObject;
                var model = new UserUpdateRequest()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    DisplayName = user.DisplayName,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email
                };
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userApiClient.Update(request);
            if (result.IsSuccessed)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        // Detail
        [HttpGet]
        public async Task<IActionResult> Detail(string userId)
        {
            var result = await _userApiClient.GetById(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                var model = result.ReturnedObject;
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }

        // Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string userId)
        {
            var result = await _userApiClient.Delete(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Error", "Home");
        }
    }
}
