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
        Task<ApiResult<PaginitionEngines<ActivityViewModel>>> Get(ActivityFilter filter);
        Task<ApiResult<ActivityViewModel>> GetById(int activityId);
        Task<ApiResult<int>> Create(ActivityCreateRequest request);
        Task<ApiResult<int>> Update(ActivityUpdateRequest request);
        Task<ApiResult<int>> Delete(int activityId);
    }
}
