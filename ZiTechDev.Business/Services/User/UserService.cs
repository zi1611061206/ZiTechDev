using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Engines.Paginition; 
using ZiTechDev.Business.Requests.User;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Business.Services.User
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
            if (!string.IsNullOrEmpty(filter.Id))
            {
                query = query.Where(x => (x.Id.ToString()).Contains(filter.Id));
            }
            if (!string.IsNullOrEmpty(filter.FullName))
            {
                query = query.Where(x => (x.FirstName + " " + x.MiddleName + " " + x.LastName).Contains(filter.FullName));
            }
            if (!string.IsNullOrEmpty(filter.UserName))
            {
                query = query.Where(x => (x.UserName).Contains(filter.UserName));
            }
            if (!string.IsNullOrEmpty(filter.DisplayName))
            {
                query = query.Where(x => (x.DisplayName).Contains(filter.DisplayName));
            }
            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(x => (x.PhoneNumber).Contains(filter.PhoneNumber));
            }
            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(x => (x.Email).Contains(filter.Email));
            }
            if (filter.Gender != -1)
            {
                query = query.Where(x => (int)x.Gender == filter.Gender);
            }
            query = query.Where(x => x.DateOfBirth > filter.FromDOB && x.DateOfBirth < filter.ToDOB);
            query = query.Where(x => x.DateOfJoin > filter.FromDOJ && x.DateOfJoin < filter.ToDOJ);

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
            var result = new PaginitionEngines<UserViewModel>()
            {
                TotalRecord = await query.CountAsync(),
                Item = data
            };
            return new Successed<PaginitionEngines<UserViewModel>>(result);
        }

        public async Task<ApiResult<UserViewModel>> GetById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Failed<UserViewModel>("Không thể tìm thấy người dùng có mã: " + userId);
            }
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
                AccessFailedCount = user.AccessFailedCount
            };
            return new Successed<UserViewModel>(viewModel);
        }

        public async Task<ApiResult<string>> Create(UserCreateRequest request)
        {
            if (await _userManager.FindByNameAsync(request.UserName) != null)
            {
                return new Failed<string>("Tên đăng nhập đã tồn tại");
            }

            var user = new AppUser()
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                DisplayName = request.DisplayName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                UserName = request.UserName
            };

            if (await _userManager.FindByEmailAsync(request.Email) != null && _userManager.IsEmailConfirmedAsync(user).Result)
            {
                return new Failed<string>("Địa chỉ email đã được đăng ký và xác thực");
            }

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new Failed<string>("Tạo mới thất bại");
            }
            return new Successed<string>(user.Id.ToString());
        }

        public async Task<ApiResult<string>> Update(UserUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return new Failed<string>("Không thể tìm thấy hành động có mã: " + request.Id);
            }
            if (await _userManager.Users.AnyAsync(x => x.Email.Equals(request.Email) && x.Id != request.Id && x.EmailConfirmed == true))
            {
                return new Failed<string>("Địa chỉ email đã được đăng ký và xác thực bởi người dùng khác");
            }

            user.FirstName = request.FirstName;
            user.MiddleName = request.MiddleName;
            user.LastName = request.LastName;
            user.DisplayName = request.DisplayName;
            user.PhoneNumber = request.PhoneNumber;
            user.Email = request.Email;
            user.DateOfBirth = request.DateOfBirth;
            user.Gender = request.Gender;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new Failed<string>("Lưu thất bại");
            }
            return new Successed<string>(user.Id.ToString());
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
    }
}
