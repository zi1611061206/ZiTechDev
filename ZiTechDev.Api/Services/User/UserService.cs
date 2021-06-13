using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Api.EmailConfiguration;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Engines.Email;
using ZiTechDev.CommonModel.Engines.Paginition;
using ZiTechDev.CommonModel.Requests.CommonItems;
using ZiTechDev.CommonModel.Requests.User;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Api.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserService(
            UserManager<AppUser> userManager,
            IConfiguration configuration,
            IEmailService emailService,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ApiResult<PaginitionEngines<UserViewModel>>> Get(UserFilter filter)
        {
            var query = _userManager.Users;
            query = Filtering(query, filter);
            query = Searching(query, filter);

            var data = await query.Skip((filter.CurrentPageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize).OrderByDescending(d => d.DateOfJoin)
                .Select(x => new UserViewModel()
                {
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    DisplayName = x.DisplayName,
                    DateOfBirth = x.DateOfBirth,
                    LastAccess = x.LastAccess,
                    DateOfJoin = x.DateOfJoin,
                    Gender = x.Gender,

                    LockoutEnd = x.LockoutEnd,
                    TwoFactorEnabled = x.TwoFactorEnabled,
                    PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                    PhoneNumber = x.PhoneNumber,
                    ConcurrencyStamp = x.ConcurrencyStamp,
                    SecurityStamp = x.SecurityStamp,
                    EmailConfirmed = x.EmailConfirmed,
                    NormalizedEmail = x.NormalizedEmail,
                    Email = x.Email,
                    NormalizedUserName = x.NormalizedUserName,
                    UserName = x.UserName,
                    Id = x.Id,
                    LockoutEnabled = x.LockoutEnabled,
                    AccessFailedCount = x.AccessFailedCount
                }).ToListAsync();
            foreach (var userViewModel in data)
            {
                var user = await _userManager.FindByIdAsync(userViewModel.Id.ToString());
                var roles = await _userManager.GetRolesAsync(user);
                userViewModel.Roles = roles;
            }
            var result = new PaginitionEngines<UserViewModel>()
            {
                TotalRecords = await query.CountAsync(),
                PageSize = filter.PageSize,
                CurrentPageIndex = filter.CurrentPageIndex,
                Item = data
            };
            return new Successed<PaginitionEngines<UserViewModel>>(result);
        }

        private static IQueryable<AppUser> Searching(IQueryable<AppUser> query, UserFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(x =>
                    x.Id.ToString().Contains(filter.Keyword) ||
                    x.FirstName.Contains(filter.Keyword) ||
                    x.MiddleName.Contains(filter.Keyword) ||
                    x.LastName.Contains(filter.Keyword) ||
                    x.UserName.Contains(filter.Keyword) ||
                    x.DisplayName.Contains(filter.Keyword) ||
                    x.PhoneNumber.Contains(filter.Keyword) ||
                    x.Email.Contains(filter.Keyword)
                );
            }
            return query;
        }

        private static IQueryable<AppUser> Filtering(IQueryable<AppUser> query, UserFilter filter)
        {
            query = query.Where(x => x.DateOfBirth > filter.FromDOB && x.DateOfBirth < filter.ToDOB);
            query = query.Where(x => x.DateOfJoin > filter.FromDOJ && x.DateOfJoin < filter.ToDOJ);
            if (!string.IsNullOrEmpty(filter.CurrentUserId))
            {
                query = query.Where(x => !x.Id.Equals(Guid.Parse(filter.CurrentUserId)));
                if (!query.Any()) return query;
            }
            if (!string.IsNullOrEmpty(filter.Id))
            {
                query = query.Where(x => x.Id.ToString().Contains(filter.Id));
                if (!query.Any()) return query;
            }
            if (!string.IsNullOrEmpty(filter.FullName))
            {
                query = query.Where(x => (x.FirstName + " " + x.MiddleName + " " + x.LastName).Contains(filter.FullName));
                if (!query.Any()) return query;
            }
            if (!string.IsNullOrEmpty(filter.UserName))
            {
                query = query.Where(x => x.UserName.Contains(filter.UserName));
                if (!query.Any()) return query;
            }
            if (!string.IsNullOrEmpty(filter.DisplayName))
            {
                query = query.Where(x => x.DisplayName.Contains(filter.DisplayName));
                if (!query.Any()) return query;
            }
            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(x => x.PhoneNumber.Contains(filter.PhoneNumber));
                if (!query.Any()) return query;
            }
            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(x => x.Email.Contains(filter.Email));
                if (!query.Any()) return query;
            }
            if (filter.Gender != -1)
            {
                query = query.Where(x => (int)x.Gender == filter.Gender);
                if (!query.Any()) return query;
            }
            if (filter.EmailConfirmed != -1)
            {
                query = query.Where(x => x.EmailConfirmed == (filter.EmailConfirmed != 0));
                if (!query.Any()) return query;
            }
            if (filter.PhoneNumberConfirmed != -1)
            {
                query = query.Where(x => x.PhoneNumberConfirmed == (filter.PhoneNumberConfirmed != 0));
                if (!query.Any()) return query;
            }
            if (filter.TwoFactorEnabled != -1)
            {
                query = query.Where(x => x.TwoFactorEnabled == (filter.TwoFactorEnabled != 0));
                if (!query.Any()) return query;
            }
            return query;
        }

        public async Task<ApiResult<UserViewModel>> GetById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Failed<UserViewModel>("Không thể tìm thấy người dùng có mã: " + userId);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new UserViewModel()
            {
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                DateOfBirth = user.DateOfBirth,
                LastAccess = user.LastAccess,
                DateOfJoin = user.DateOfJoin,
                Gender = user.Gender,

                LockoutEnd = user.LockoutEnd,
                TwoFactorEnabled = user.TwoFactorEnabled,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                PhoneNumber = user.PhoneNumber,
                ConcurrencyStamp = user.ConcurrencyStamp,
                SecurityStamp = user.SecurityStamp,
                EmailConfirmed = user.EmailConfirmed,
                NormalizedEmail = user.NormalizedEmail,
                Email = user.Email,
                NormalizedUserName = user.NormalizedUserName,
                UserName = user.UserName,
                Id = user.Id,
                LockoutEnabled = user.LockoutEnabled,
                AccessFailedCount = user.AccessFailedCount,
                Roles = roles
            };
            return new Successed<UserViewModel>(viewModel);
        }

        public async Task<ApiResult<string>> Create(UserCreateRequest request)
        {
            if (await _userManager.FindByNameAsync(request.UserName) != null)
            {
                return new Failed<string>("Tên đăng nhập đã tồn tại");
            }

            if (string.IsNullOrEmpty(request.DisplayName))
            {
                request.DisplayName = request.UserName;
            }

            var user = new AppUser()
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                DisplayName = request.DisplayName,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                UserName = request.UserName,
                Gender = request.Gender
            };

            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new Failed<string>("Địa chỉ email đã được đăng ký");
            }

            var result = await _userManager.CreateAsync(user, new PasswordGenerator().Generate());
            if (!result.Succeeded)
            {
                return new Failed<string>("Tạo mới thất bại");
            }
            await RoleAssign(user.Id, request.Roles);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            return new Successed<string>(encodedToken);
        }

        public async Task<ApiResult<string>> Update(UserUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return new Failed<string>("Không thể tìm thấy người dùng có mã: " + request.Id);
            }
            if (await _userManager.Users.AnyAsync(x => x.Email.Equals(request.Email) && x.Id != request.Id))
            {
                return new Failed<string>("Địa chỉ email đã được đăng ký và xác thực bởi người dùng khác");
            }

            if (!string.IsNullOrEmpty(request.DisplayName))
            {
                user.DisplayName = request.DisplayName;
            }
            if (!user.Email.ToLower().Equals(request.Email.ToLower()))
            {
                user.Email = request.Email;
                user.EmailConfirmed = false;
            }
            user.FirstName = request.FirstName;
            user.MiddleName = request.MiddleName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.DateOfBirth = request.DateOfBirth;
            user.Gender = request.Gender;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new Failed<string>("Lưu thất bại");
            }
            await RoleAssign(user.Id, request.Roles);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            return new Successed<string>(encodedToken);
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid userId, List<RoleItem> roles)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Failed<bool>("Tài khoản không tồn tại");
            }

            var nonCheckedRoles = roles.Where(x => x.Checked == false).Select(x => x.Name).ToList();
            foreach (var roleName in nonCheckedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }
            await _userManager.RemoveFromRolesAsync(user, nonCheckedRoles);

            var checkedRoles = roles.Where(x => x.Checked).Select(x => x.Name).ToList();
            foreach (var roleName in checkedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return new Successed<bool>(true);
        }

        public async Task<ApiResult<bool>> Delete(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Failed<bool>("Không thể tìm thấy người dùng có mã: " + userId);
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return new Failed<bool>("Xóa thất bại");
            }
            return new Successed<bool>(true);
        }

        public async Task<ApiResult<string>> ConfirmEmail(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Failed<string>("Người dùng không tồn tại");
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            return new Successed<string>(encodedToken);
        }

        public async Task<ApiResult<UserViewModel>> GetByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new Failed<UserViewModel>("Không thể tìm thấy người dùng " + userName);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new UserViewModel()
            {
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                DateOfBirth = user.DateOfBirth,
                LastAccess = user.LastAccess,
                DateOfJoin = user.DateOfJoin,
                Gender = user.Gender,

                LockoutEnd = user.LockoutEnd,
                TwoFactorEnabled = user.TwoFactorEnabled,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                PhoneNumber = user.PhoneNumber,
                ConcurrencyStamp = user.ConcurrencyStamp,
                SecurityStamp = user.SecurityStamp,
                EmailConfirmed = user.EmailConfirmed,
                NormalizedEmail = user.NormalizedEmail,
                Email = user.Email,
                NormalizedUserName = user.NormalizedUserName,
                UserName = user.UserName,
                Id = user.Id,
                LockoutEnabled = user.LockoutEnabled,
                AccessFailedCount = user.AccessFailedCount,
                Roles = roles
            };
            return new Successed<UserViewModel>(viewModel);
        }

        public async Task<ApiResult<bool>> SendActiveEmail(string emailAddress, string token, string activeBaseUrl)
        {
            var user = await _userManager.FindByEmailAsync(emailAddress);
            var activeUrl = activeBaseUrl + $"?userId={user.Id}&token={token}";

            var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
            template.EmailConfirmation(activeUrl, user.UserName);

            var email = new EmailItem();
            email.Senders.Add(new EmailBase(_configuration.GetValue<string>("EmailSender:Name"), _configuration.GetValue<string>("EmailSender:Address")));
            email.Receivers.Add(new EmailBase(user.UserName, user.Email));
            email.Subject = template.Subject;
            email.Body = template.Content;
            if(await _emailService.SendAsync(email))
            {
                return new Successed<bool>(true);
            }
            return new Failed<bool>(string.Empty);
        }
    }
}
