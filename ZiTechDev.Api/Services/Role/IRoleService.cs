using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Engines.Paginition;
using ZiTechDev.CommonModel.Requests.Role;

namespace ZiTechDev.Api.Services.Role
{
    public interface IRoleService
    {
        Task<ApiResult<List<RoleViewModel>>> GetAll();
        Task<ApiResult<PaginitionEngines<RoleViewModel>>> Get(RoleFilter filter);
        Task<ApiResult<RoleViewModel>> GetById(Guid userId);
        Task<ApiResult<string>> Create(RoleCreateRequest request);
        Task<ApiResult<string>> Update(RoleUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid userId);
    }
}
