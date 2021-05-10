﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZiTechDev.Business.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsExistedUserName(string userName);
        Task<bool> IsMatchedUser(string userName, string password, bool rememberMe);
    }
}
