using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.Exceptions;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.User;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Business.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> Create(UserCreateRequest request)
        {
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
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return null;
            }
            return user.Id.ToString();
        }

        public async Task<bool> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ExceptionEngines($"Can not find with ID: {userId}");
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }

        public async Task<PaginitionEngines<UserViewModel>> GetAll(UserFilter filter)
        {
            var query = _userManager.Users;
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

            var data = await query.Skip((filter.CurrentPageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize).OrderByDescending(d=>d.DateOfJoin)
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
            return result;
        }

        public async Task<bool> Update(UserUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                throw new ExceptionEngines($"Can not find with ID: {request.Id}");
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
                return false;
            }
            return true;
        }

        public async Task<bool> IsExistedUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsMatchedUser(string userName, string password, bool rememberMe)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return false;
            }
            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, true);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }
    }
}
