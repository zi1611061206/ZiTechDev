using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Requests.Auth;

namespace ZiTechDev.AdminSite.ApiClientServices.Auth
{
    public interface IAuthApiClient
    {
        Task<ApiResult<string>> Login(LoginRequest request);
    }
}
