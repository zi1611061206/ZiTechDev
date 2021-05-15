using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Requests.Activity;
using ZiTechDev.Data.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZiTechDev.Data.Context;
using ZiTechDev.Business.Engines.CustomResult;

namespace ZiTechDev.Business.Services.Activities
{
    public class ActivityService : IActivityService
    {
        private readonly ZiTechDevDBContext _context;

        public ActivityService(ZiTechDevDBContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<PaginitionEngines<ActivityViewModel>>> Get(ActivityFilter filter)
        {
            var query = from a in _context.Activities select a;
            if (filter.Id != 0)
            {
                query = query.Where(x => x.Id == filter.Id);
            }
            if (filter.Name != null)
            {
                query = query.Where(x => x.Name.Contains(filter.Name));
            }
            if (filter.FunctionIds.Count > 0)
            {
                query = query.Where(x => filter.FunctionIds.Contains(x.FunctionId));
            }
            var data = await query.Skip((filter.CurrentPageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize).OrderBy(d => d.Id)
                .Select(x => new ActivityViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    FunctionId = x.FunctionId
                }).ToListAsync();
            var result = new PaginitionEngines<ActivityViewModel>()
            {
                TotalRecord = await query.CountAsync(),
                Item = data
            };
            return new Successed<PaginitionEngines<ActivityViewModel>>(result);
        }

        public async Task<ApiResult<ActivityViewModel>> GetById(int activityId)
        {
            var activity = await _context.Activities.FindAsync(activityId);
            if (activity == null)
            {
                return new Failed<ActivityViewModel>("Không thể tìm thấy hành động có mã: " + activityId);
            }
            var viewModel = new ActivityViewModel()
            {
                Id = activity.Id,
                Name = activity.Name,
                Description = activity.Description,
                FunctionId = activity.FunctionId
                
            };
            return new Successed<ActivityViewModel>(viewModel);
        }

        public async Task<ApiResult<int>> Create(ActivityCreateRequest request)
        {
            var query = from a in _context.Activities select a;
            var data = query.Where(x => x.Name.Equals(request.Name) && x.FunctionId == request.FunctionId).ToList();
            if(data.Count > 0)
            {
                return new Failed<int>("Tên hành động đã tồn tại trong cùng hàm");
            }

            var activity = new Activity()
            {
                Name = request.Name,
                Description = request.Description,
                FunctionId = request.FunctionId
            };
            _context.Activities.Add(activity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return new Failed<int>("Lưu thất bại với lỗi: " + e.Message);
            }
            return new Successed<int>(activity.Id);
        }

        public async Task<ApiResult<int>> Update(ActivityUpdateRequest request)
        {
            var query = from a in _context.Activities select a;
            var data = query.Where(x => x.Name.Equals(request.Name) && x.FunctionId == request.FunctionId && x.Id == request.Id).ToList();
            if (data.Count > 0)
            {
                return new Failed<int>("Tên hành động đã tồn tại trong cùng hàm");
            }
            var activity = await _context.Activities.FindAsync(request.Id);
            if (activity == null)
            {
                return new Failed<int>("Không thể tìm thấy hành động có mã: " + request.Id);
            }

            activity.Name = request.Name;
            activity.Description = request.Description;
            activity.FunctionId = request.FunctionId;
            _context.Activities.Update(activity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new Failed<int>("Lưu thất bại với lỗi: " + e.Message);
            }
            return new Successed<int>(activity.Id);
        }

        public async Task<ApiResult<int>> Delete(int activityId)
        {
            var activity = await _context.Activities.FindAsync(activityId);
            if (activity == null)
            {
                return new Failed<int>("Không thể tìm thấy hành động có mã: " + activityId);
            }

            _context.Activities.Remove(activity);

            int result;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new Failed<int>("Xóa thất bại với lỗi: " + e.Message);
            }
            return new Successed<int>(result);
        }
    }
}
