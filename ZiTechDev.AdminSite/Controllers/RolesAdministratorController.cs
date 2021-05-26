using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.AdminSite.ApiClientServices.Role;
using ZiTechDev.Business.Requests.Role;

namespace ZiTechDev.AdminSite.Controllers
{
    public class RolesAdministratorController : BaseController
    {
        private readonly IRoleApiClient _roleApiClient;

        public RolesAdministratorController(IRoleApiClient roleApiClient)
        {
            _roleApiClient = roleApiClient;
        }

        // Read
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var filter = new RoleFilter();
            var result = await _roleApiClient.Get(filter);

            ViewData["Roles"] = result.ReturnedObject;
            ViewData["Filter"] = filter;

            ViewBag.Title = "Danh sách vai trò";
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }
            if (TempData["Fail"] != null)
            {
                ViewBag.Fail = TempData["Fail"];
            }
            if (result.ReturnedObject.TotalRecords == 0)
            {
                ViewBag.Info = "Không có vai trò nào";
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RoleFilter filter)
        {
            var result = await _roleApiClient.Get(filter);

            ViewData["Roles"] = result.ReturnedObject;
            ViewData["Filter"] = filter;

            ViewBag.Title = "Danh sách vai trò";
            if (result.ReturnedObject.TotalRecords == 0)
            {
                ViewBag.Info = "Không có vai trò nào";
            }

            return View();
        }

        // Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new RoleCreateRequest();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _roleApiClient.Create(request);

            if (result.IsSuccessed)
            {
                TempData["Success"] = "Tạo mới thành công vai trò có mã: " + result.ReturnedObject;
                ModelState.Clear();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        // Update
        [HttpGet]
        public async Task<IActionResult> Update(string roleId)
        {
            var result = await _roleApiClient.GetById(Guid.Parse(roleId));
            if (result.IsSuccessed)
            {
                var role = result.ReturnedObject;
                var model = new RoleUpdateRequest()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                };
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoleUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _roleApiClient.Update(request);
            if (result.IsSuccessed)
            {
                TempData["Success"] = "Cập nhật thành công vai trò có mã: " + result.ReturnedObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        // Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string roleId)
        {
            var result = await _roleApiClient.Delete(Guid.Parse(roleId));
            if (result.IsSuccessed)
            {
                TempData["Success"] = "Xóa vai trò thành công";
                return RedirectToAction("Index");
            }
            TempData["Fail"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}
