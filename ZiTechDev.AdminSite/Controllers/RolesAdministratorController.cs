using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ZiTechDev.AdminSite.ApiClientServices.Role;
using ZiTechDev.CommonModel.Requests.Role;

namespace ZiTechDev.AdminSite.Controllers
{
    public class RolesAdministratorController : BaseController
    {
        private readonly IRoleApiClient _roleApiClient;

        public RolesAdministratorController(IRoleApiClient roleApiClient)
        {
            _roleApiClient = roleApiClient;
        }

        #region Read Role List
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var filter = new RoleFilter();
            var result = await _roleApiClient.Get(filter);

            ViewData["Roles"] = result.ReturnedObject;
            ViewData["Filter"] = filter;

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
            ViewBag.Title = "Danh sách vai trò";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RoleFilter filter)
        {
            var result = await _roleApiClient.Get(filter);

            ViewData["Roles"] = result.ReturnedObject;
            ViewData["Filter"] = filter;

            if (result.ReturnedObject.TotalRecords == 0)
            {
                ViewBag.Info = "Không có vai trò nào";
            }
            ViewBag.Title = "Danh sách vai trò";
            return View();
        }
        #endregion

        #region Create Role
        [HttpGet]
        public IActionResult Create()
        {
            var model = new RoleCreateRequest();
            ViewBag.Title = "Tạo mới vai trò";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Tạo mới vai trò";
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
            ViewBag.Title = "Tạo mới vai trò";
            return View(request);
        }
        #endregion

        #region Update Role
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
                ViewBag.Title = "Cập nhật vai trò";
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoleUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Cập nhật vai trò";
                return View(request);
            }

            var result = await _roleApiClient.Update(request);
            if (result.IsSuccessed)
            {
                TempData["Success"] = "Cập nhật thành công vai trò có mã: " + result.ReturnedObject;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            ViewBag.Title = "Cập nhật vai trò";
            return View(request);
        }
        #endregion

        #region Delete Role
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
        #endregion
    }
}
