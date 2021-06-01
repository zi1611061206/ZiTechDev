using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Requests.Auth;

namespace ZiTechDev.Business.Services.Auth
{
    public interface IAuthService
    {
        Task<ApiResult<string>> Login(LoginRequest request);
        Task<ApiResult<string>> Register(RegisterRequest request);
        Task<ApiResult<bool>> EditProfile(EditProfileRequest request);
        Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request);
    }
}
