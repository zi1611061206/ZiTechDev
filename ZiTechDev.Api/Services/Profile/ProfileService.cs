using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Requests.Profile;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Api.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        #region GetProfile (Handler)
        public async Task<ApiResult<ProfileViewModel>> GetProfile(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Failed<ProfileViewModel>("Không thể tìm thấy người dùng có mã: " + userId);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new ProfileViewModel()
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
            return new Successed<ProfileViewModel>(viewModel);
        }
        #endregion

        #region EditProfile (Handler)
        public async Task<ApiResult<bool>> EditProfile(ProfileEditRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return new Failed<bool>("Không thể tìm thấy người dùng có mã: " + request.Id);
            }
            if (await _userManager.Users.AnyAsync(x => x.Email.Equals(request.Email) && x.Id != request.Id))
            {
                return new Failed<bool>("Địa chỉ email đã được đăng ký bởi người dùng khác");
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
                return new Failed<bool>("Lưu thất bại");
            }
            return new Successed<bool>(true);
        }
        #endregion

        #region ChangePassword (Handler)
        public async Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return new Failed<bool>("Không thể tìm thấy người dùng có mã: " + request.Id);
            }
            if (await _userManager.CheckPasswordAsync(user, request.OldPassword))
            {
                var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
                if (!result.Succeeded)
                {
                    return new Failed<bool>("Thay đổi mật khẩu thất bại");
                }
                return new Successed<bool>(true);
            }
            else
            {
                return new Failed<bool>("Mật khẩu hiện tại không đúng");
            }
        }
        #endregion
    }
}
