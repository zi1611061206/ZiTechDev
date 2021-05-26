using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.Role;

namespace ZiTechDev.Business.Services.Role
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
