using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.CustomResult;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.Role;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Business.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApiResult<List<RoleViewModel>>> GetAll()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                NormalizedName = x.NormalizedName,
                ConcurrencyStamp = x.ConcurrencyStamp
            }).ToListAsync();

            return new Successed<List<RoleViewModel>>(roles);
        }

        public async Task<ApiResult<PaginitionEngines<RoleViewModel>>> Get(RoleFilter filter)
        {
            var query = _roleManager.Roles;
            query = Filtering(query, filter);
            query = Searching(query, filter);

            var data = await query.Skip((filter.CurrentPageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize).OrderByDescending(d => d.Id)
                .Select(x => new RoleViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ConcurrencyStamp = x.ConcurrencyStamp,
                    NormalizedName = x.NormalizedName
                }).ToListAsync();
            var result = new PaginitionEngines<RoleViewModel>()
            {
                TotalRecords = await query.CountAsync(),
                PageSize = filter.PageSize,
                CurrentPageIndex = filter.CurrentPageIndex,
                Item = data
            };
            return new Successed<PaginitionEngines<RoleViewModel>>(result);
        }

        private IQueryable<AppRole> Searching(IQueryable<AppRole> query, RoleFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(x =>
                    x.Id.ToString().Contains(filter.Keyword) ||
                    x.Name.Contains(filter.Keyword) ||
                    x.Description.Contains(filter.Keyword) ||
                    x.NormalizedName.Contains(filter.Keyword) ||
                    x.ConcurrencyStamp.Contains(filter.Keyword)
                );
            }
            return query;
        }

        private IQueryable<AppRole> Filtering(IQueryable<AppRole> query, RoleFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Id))
            {
                query = query.Where(x => (x.Id.ToString()).Contains(filter.Id));
            }
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => (x.Name).Contains(filter.Name));
            }
            return query;
        }

        public async Task<ApiResult<RoleViewModel>> GetById(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return new Failed<RoleViewModel>("Không thể tìm thấy vai trò có mã: " + roleId);
            }
            var viewModel = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                ConcurrencyStamp = role.ConcurrencyStamp,
                NormalizedName = role.NormalizedName
            };
            return new Successed<RoleViewModel>(viewModel);
        }

        public async Task<ApiResult<string>> Create(RoleCreateRequest request)
        {
            if (await _roleManager.FindByNameAsync(request.Name) != null)
            {
                return new Failed<string>("Tên vai trò đã tồn tại");
            }

            var role = new AppRole()
            {
                Name = request.Name,
                Description = request.Description
            };

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return new Failed<string>("Tạo mới thất bại");
            }
            return new Successed<string>(role.Id.ToString());
        }

        public async Task<ApiResult<string>> Update(RoleUpdateRequest request)
        {
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            if (role == null)
            {
                return new Failed<string>("Không thể tìm thấy vai trò có mã: " + request.Id);
            }

            role.Name = request.Name;
            role.Description = request.Description;

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return new Failed<string>("Lưu thất bại");
            }
            return new Successed<string>(role.Id.ToString());
        }

        public async Task<ApiResult<bool>> Delete(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return new Failed<bool>("Không thể tìm thấy vai trò có mã: " + roleId);
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                return new Failed<bool>("Xóa thất bại");
            }
            return new Successed<bool>(true);
        }
    }
}
