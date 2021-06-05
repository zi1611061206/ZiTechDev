using System;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Engines.Paginition;
using ZiTechDev.CommonModel.Requests.User;

namespace ZiTechDev.AdminSite.ApiClientServices.User
{
    public interface IUserApiClient
    {
        Task<ApiResult<PaginitionEngines<UserViewModel>>> Get(UserFilter filter);
        Task<ApiResult<UserViewModel>> GetById(Guid userId);
        Task<ApiResult<string>> Create(UserCreateRequest request);
        Task<ApiResult<string>> Update(UserUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid userId);
        Task<ApiResult<string>> ResetPassword(Guid userId);
    }
}
