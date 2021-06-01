using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Requests.Auth;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.AdminSite.ApiClientServices.Auth
{
    public interface IAuthApiClient
    {
        Task<ApiResult<string>> Login(LoginRequest request);
        Task<ApiResult<UserViewModel>> GetProfile(Guid userId);
        Task<ApiResult<bool>> EditProfile(EditProfileRequest request);
        Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request);
    }
}
