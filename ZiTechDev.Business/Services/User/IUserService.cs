using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.User;

namespace ZiTechDev.Business.Services.User
{
    public interface IUserService
    {
        Task<string> Create(UserCreateRequest request);
        Task<bool> Update(UserUpdateRequest request);
        Task<bool> Delete(string userId);
        Task<PaginitionEngines<UserViewModel>> GetAll(UserFilter filter);
        Task<bool> IsExistedUserName(string userName);
        Task<bool> IsMatchedUser(string userName, string password, bool rememberMe);
    }
}
