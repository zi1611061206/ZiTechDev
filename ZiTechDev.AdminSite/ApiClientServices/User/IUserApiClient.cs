using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.Auth;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.AdminSite.ApiClientServices.User
{
    public interface IUserApiClient
    {
        Task<ApiResult<PaginitionEngines<UserViewModel>>> Get(UserFilter filter);
        Task<ApiResult<UserViewModel>> GetById(Guid userId);
        Task<ApiResult<string>> Create(UserCreateRequest request);
        Task<ApiResult<string>> Update(UserUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid userId);
    }
}
