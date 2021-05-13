using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Requests.Auth;

namespace ZiTechDev.Business.Services.Auth
{
    public interface IAuthService
    {
        Task<string> Login(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task<bool> IsExistedUserName(string userName);
        Task<bool> IsMatchedUser(string userName, string password, bool rememberMe);
    }
}
