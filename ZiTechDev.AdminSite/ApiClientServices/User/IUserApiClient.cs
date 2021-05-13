using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.Auth;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.AdminSite.ApiClientServices.User
{
    public interface IUserApiClient
    {
        Task<PaginitionEngines<UserViewModel>> Get(UserFilter filter);
        Task<bool> Create(UserCreateRequest request);
    }
}
