using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.Role;

namespace ZiTechDev.AdminSite.ApiClientServices.Role
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleViewModel>>> GetAll();
        Task<ApiResult<PaginitionEngines<RoleViewModel>>> Get(RoleFilter filter);
        Task<ApiResult<RoleViewModel>> GetById(Guid roleId);
        Task<ApiResult<string>> Create(RoleCreateRequest request);
        Task<ApiResult<string>> Update(RoleUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid roleId);
    }
}
