using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.AdminSite.ApiClientServices.Profile
{
    public interface IProfileApiClient
    {
        Task<ApiResult<UserViewModel>> GetProfile(Guid userId);
    }
}
