using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.User;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Business.Services.User
{
    public interface IUserService
    {
        Task<ApiResult<PaginitionEngines<UserViewModel>>> Get(UserFilter filter);
        Task<ApiResult<UserViewModel>> GetById(Guid userId);
        Task<ApiResult<string>> Create(UserCreateRequest request);
        Task<ApiResult<string>> Update(UserUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid roleId);
        Task<ApiResult<string>> ResetPassword(Guid userId);
    }
}
