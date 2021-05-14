using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.Activity;

namespace ZiTechDev.Business.Services.Activities
{
    public interface IActivityService
    {
        Task<ApiResult<int>> Create(ActivityCreateRequest request);
        Task<ApiResult<int>> Update(ActivityUpdateRequest request);
        Task<ApiResult<int>> Delete(int activityId);
        Task<ApiResult<PaginitionEngines<ActivityViewModel>>> GetAll(ActivityFilter filter);
    }
}
