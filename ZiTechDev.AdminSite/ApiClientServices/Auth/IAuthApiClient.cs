using System;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Requests.Auth;
using ZiTechDev.CommonModel.Requests.User;

namespace ZiTechDev.AdminSite.ApiClientServices.Auth
{
    public interface IAuthApiClient
    {
        Task<ApiResult<string>> Login(LoginRequest request);
        Task<ApiResult<UserViewModel>> GetProfile(Guid userId);
        Task<ApiResult<UserViewModel>> GetByEmail(string email);
        Task<ApiResult<bool>> EditProfile(EditProfileRequest request);
        Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request);
        Task<ApiResult<bool>> VertifiedEmail(Guid userId, string token);
        Task<ApiResult<string>> ForgotPassword(ForgotPasswordRequest request);
        Task<ApiResult<bool>> ResetPassword(ResetPasswordRequest request);
    }
}
