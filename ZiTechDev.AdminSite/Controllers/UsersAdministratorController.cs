using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.AdminSite.ApiClientServices.Role;
using ZiTechDev.AdminSite.ApiClientServices.User;
using ZiTechDev.AdminSite.EmailConfiguration;
using ZiTechDev.CommonModel.Engines.Email;
using ZiTechDev.CommonModel.Requests.User;

namespace ZiTechDev.AdminSite.Controllers
{
    public class UsersAdministratorController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IRoleApiClient _roleApiClient;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public UsersAdministratorController(
            IUserApiClient userApiClient, 
            IRoleApiClient roleApiClient,
            IEmailService emailService,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _roleApiClient = roleApiClient;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        #region Read Memeber List
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var filter = new UserFilter();

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
            ViewBag.Title = "Danh sách thành viên";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserFilter filter)
        {
            var result = await _userApiClient.Get(filter);

            ViewData["Users"] = result.ReturnedObject;
            ViewData["Filter"] = filter;

            ViewBag.Title = "Danh sách thành viên";
            if (result.ReturnedObject.TotalRecords == 0)
            {
                ViewBag.Info = "Không có người dùng nào";
            }
            ViewBag.Title = "Danh sách thành viên";
            return View();
        }
        #endregion

        #region Create Member
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
            ViewBag.Title = "Tạo mới thành viên";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Tạo mới thành viên";
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
                var user = await _userApiClient.GetByUserName(request.UserName);
                var emailConfirmUrl = Url.ActionLink("VertifiedEmail", "Auth", new { userId = user.ReturnedObject.Id, token = result.ReturnedObject }, Request.Scheme);
                var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
                template.EmailConfirmation(emailConfirmUrl, request.UserName);

                var email = new EmailItem();
                email.Senders.Add(new EmailBase(_configuration.GetValue<string>("EmailSender:Name"), _configuration.GetValue<string>("EmailSender:Address")));
                email.Receivers.Add(new EmailBase(request.UserName, request.Email));
                email.Subject = template.Subject;
                email.Body = template.Content;
                await _emailService.SendAsync(email);

                ModelState.Clear();
                TempData["Success"] = "Tạo mới thành công! (Chờ xác thực)";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            ViewBag.Title = "Tạo mới thành viên";
            return View(request);
        }
        #endregion

        #region Update Member
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
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Gender = user.Gender
                };
                var roleObj = await _roleApiClient.GetAll(); 
                foreach (var role in roleObj.ReturnedObject)
                {
                    model.Roles.Add(new RoleItem()
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Checked = result.ReturnedObject.Roles.Contains(role.Name)
                    });
                }
                ViewBag.Title = "Cập nhật hồ sơ thành viên";
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Cập nhật hồ sơ thành viên";
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
                var user = await _userApiClient.GetById(request.Id);
                var emailConfirmUrl = Url.ActionLink("VertifiedEmail", "Auth", new { userId = request.Id, token = result.ReturnedObject }, Request.Scheme);
                var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
                template.EmailConfirmation(emailConfirmUrl, user.ReturnedObject.UserName);

                var email = new EmailItem();
                email.Senders.Add(new EmailBase(_configuration.GetValue<string>("EmailSender:Name"), _configuration.GetValue<string>("EmailSender:Address")));
                email.Receivers.Add(new EmailBase(user.ReturnedObject.UserName, user.ReturnedObject.Email));
                email.Subject = template.Subject;
                email.Body = template.Content;
                await _emailService.SendAsync(email);

                ModelState.Clear();
                TempData["Success"] = "Cập nhật người dùng thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            ViewBag.Title = "Cập nhật hồ sơ thành viên";
            return View(request);
        }
        #endregion

        #region Detail Member
        [HttpGet]
        public async Task<IActionResult> Detail(string userId)
        {
            var result = await _userApiClient.GetById(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                var model = result.ReturnedObject;
                ViewBag.Title = "Hồ sơ thành viên";
                return View(model);
            }
            TempData["Fail"] = result.Message;
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete Member
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
        #endregion

        #region Confirm Email
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId)
        {
            var result = await _userApiClient.ConfirmEmail(Guid.Parse(userId));
            if (result.IsSuccessed)
            {
                var user = await _userApiClient.GetById(Guid.Parse(userId));
                var emailConfirmUrl = Url.ActionLink("VertifiedEmail", "Auth", new { userId, token = result.ReturnedObject }, Request.Scheme);
                var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
                template.EmailConfirmation(emailConfirmUrl, user.ReturnedObject.UserName);

                var email = new EmailItem();
                email.Senders.Add(new EmailBase(_configuration.GetValue<string>("EmailSender:Name"), _configuration.GetValue<string>("EmailSender:Address")));
                email.Receivers.Add(new EmailBase(user.ReturnedObject.UserName, user.ReturnedObject.Email));
                email.Subject = template.Subject;
                email.Body = template.Content;
                await _emailService.SendAsync(email);

                TempData["Success"] = "Đã gửi xác thực đến email người dùng";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Error", "Home");
        }
        #endregion
    }
}
