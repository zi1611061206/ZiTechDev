using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.Activity;

namespace ZiTechDev.Business.Interfaces
{
    public interface IActivityService
    {
        Task<int> Create(ActivityCreateRequest request);
        Task<int> Update(ActivityUpdateRequest request);
        Task<int> Delete(int activityId);
        Task<PaginitionEngines<ActivityViewModel>> GetAll(ActivityFilter filter);
    }
}
