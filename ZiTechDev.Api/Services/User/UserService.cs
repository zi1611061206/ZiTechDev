using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Engines.Paginition;
using ZiTechDev.CommonModel.Requests.Auth;
using ZiTechDev.CommonModel.Requests.User;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Api.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new Failed<string>("Tạo mới thất bại");
            }
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));
            await RoleAssign(user.Id, request.Roles);
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
            user.FirstName = request.FirstName;
            user.MiddleName = request.MiddleName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.Email = request.Email;
            user.DateOfBirth = request.DateOfBirth;
            user.Gender = request.Gender;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new Failed<string>("Lưu thất bại");
            }
            await RoleAssign(user.Id, request.Roles);
            return new Successed<string>(user.Id.ToString());
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

        public async Task<ApiResult<string>> ResetPassword(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Failed<string>("Người dùng không tồn tại");
            }

            var generator = new PasswordGenerator();
            string password = generator.Generate();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (!result.Succeeded)
            {
                return new Failed<string>("Đặt lại mật khẩu thất bại. Vui lòng thử lại.");
            }

            return new Successed<string>(password);
        }
    }
}
