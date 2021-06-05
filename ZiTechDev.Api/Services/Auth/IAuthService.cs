using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.Api.Services.Auth
{
    public interface IAuthService
    {
        Task<ApiResult<string>> Login(LoginRequest request);
        Task<ApiResult<string>> Register(RegisterRequest request);
        Task<ApiResult<bool>> EditProfile(EditProfileRequest request);
        Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request);
        Task<ApiResult<bool>> ConfirmEmail(string userName, string token);
    }
}
