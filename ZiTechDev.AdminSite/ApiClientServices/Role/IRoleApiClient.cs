using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Engines.Paginition;
using ZiTechDev.CommonModel.Requests.Role;

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
