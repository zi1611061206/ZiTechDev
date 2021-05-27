using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.AdminSite.ApiClientServices.Role;
using ZiTechDev.AdminSite.ApiClientServices.User;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.Role;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.AdminSite.Controllers
{
    public class UsersAdministratorController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IRoleApiClient _roleApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersAdministratorController(
            IUserApiClient userApiClient, 
            IRoleApiClient roleApiClient, 
            IHttpContextAccessor httpContextAccessor)
        {
            _userApiClient = userApiClient;
            _roleApiClient = roleApiClient;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == "UserId").Value;
        }

        // Read
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var filter = new UserFilter();
            filter.CurrentUserId = GetCurrentUserId();

            var result = await _userApiClient.Get(filter);
            var roles = _roleApiClient.GetAll().Result;
            if (roles.IsSuccessed)
            {
                foreach (var role in roles.ReturnedObject)
                {
                    filter.Roles.Add(new RoleItem()
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Checked = true
                    });
                }
            }

            ViewData["Users"] = result.ReturnedObject;
            ViewData["Filter"] = filter;

            ViewBag.Title = "Danh sách thành viên";
            if(TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }
            if (TempData["Fail"] != null)
            {
                ViewBag.Fail = TempData["Fail"];
            }
            if (result.ReturnedObject.TotalRecords == 0)
            {
                ViewBag.Info = "Không có người dùng nào";
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserFilter filter)
        {
            filter.CurrentUserId = GetCurrentUserId();
            var result = await _userApiClient.Get(filter);

            ViewData["Users"] = result.ReturnedObject;
            ViewData["Filter"] = filter;

            ViewBag.Title = "Danh sách thành viên";
            if (result.ReturnedObject.TotalRecords == 0)
            {
                ViewBag.Info = "Không có người dùng nào";
            }

            return View();
        }

        // Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new UserCreateRequest();
            var roles = _roleApiClient.GetAll().Result;
            if (roles.IsSuccessed)
            {
                foreach(var role in roles.ReturnedObject)
                {
                    model.Roles.Add(new RoleItem() { 
                        Id = role.Id,
                        Name = role.Name,
                        Checked = role.Name.Contains("User")
                    });
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            foreach(var item in request.Roles)
            {
                if(item.Name.Equals("User") && !item.Checked)
                {
                    item.Checked = true;
                }
            }

            var result = await _userApiClient.Create(request);

            if (result.IsSuccessed)
            {
                TempData["Success"] = "Tạo mới thành công người dùng có mã: " + result.ReturnedObject;
                ModelState.Clear();
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
                var userObj = await _userApiClient.GetById(user.Id);
                var roleObj = await _roleApiClient.GetAll(); 
                foreach (var role in roleObj.ReturnedObject)
                {
                    model.Roles.Add(new RoleItem()
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Checked = userObj.ReturnedObject.Roles.Contains(role.Name)
                    });
                }
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

            foreach (var item in request.Roles)
            {
                if (item.Name.Equals("User") && !item.Checked)
                {
                    item.Checked = true;
                }
            }

            var result = await _userApiClient.Update(request);
            if (result.IsSuccessed)
            {
                TempData["Success"] = "Cập nhật thành công người dùng có mã: " + result.ReturnedObject;
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
            TempData["Fail"] = result.Message;
            return RedirectToAction("Index");
        }

        // Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string userId)
        {
            var result = await _userApiClient.Delete(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                TempData["Success"] = "Xóa người dùng thành công";
                return RedirectToAction("Index");
            }
            TempData["Fail"] = result.Message;
            return RedirectToAction("Index");
        }

        // ResetPassword
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userId)
        {
            var result = await _userApiClient.ResetPassword(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                TempData["Success"] = "Đặt lại mật khẩu thành công, mật khẩu mới của bạn là " + result.ReturnedObject;
                return RedirectToAction("Index");
            }
            TempData["Fail"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}
