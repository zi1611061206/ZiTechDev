﻿using System;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.CustomResult;
using ZiTechDev.CommonModel.Engines.Paginition;
using ZiTechDev.CommonModel.Requests.User;

namespace ZiTechDev.Api.Services.User
{
    public interface IUserService
    {
        Task<ApiResult<PaginitionEngines<UserViewModel>>> Get(UserFilter filter);
        Task<ApiResult<UserViewModel>> GetById(Guid userId);
        Task<ApiResult<string>> Create(UserCreateRequest request);
        Task<ApiResult<string>> Update(UserUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid roleId);
        Task<ApiResult<string>> ResetPassword(Guid userId);
    }
}