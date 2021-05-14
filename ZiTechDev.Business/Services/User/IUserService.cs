using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.Business.Services.User
{
    public interface IUserService
    {
        Task<ApiResult<string>> Create(UserCreateRequest request);
        Task<ApiResult<string>> Update(UserUpdateRequest request);
        Task<ApiResult<bool>> Delete(string userId);
        Task<ApiResult<PaginitionEngines<UserViewModel>>> GetAll(UserFilter filter);
    }
}
