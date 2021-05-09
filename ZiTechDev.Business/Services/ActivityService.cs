using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.Exceptions;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Business.Interfaces;
using ZiTechDev.Business.Requests.Activity;
using ZiTechDev.Data.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZiTechDev.Data.Context;

namespace ZiTechDev.Business.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ZiTechDevDBContext _context;
        public ActivityService(ZiTechDevDBContext context)
        {
            _context = context;
        }

        public async Task<int> Create(ActivityCreateRequest request)
        {
            var activity = new Activity()
            {
                Name = request.Name,
                Description = request.Description,
                FunctionId = request.FunctionId
            };
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity.Id;
        }

        public async Task<int> Update(ActivityUpdateRequest request)
        {
            var activity = await _context.Activities.FindAsync(request.Id);
            if (activity == null)
            {
                throw new ExceptionEngines($"Can not find with ID: {request.Id}");
            }
            activity.Name = request.Name;
            activity.Description = request.Description;
            activity.FunctionId = request.FunctionId;
            _context.Activities.Update(activity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int activityId)
        {
            var activity = await _context.Activities.FindAsync(activityId);
            if (activity == null)
            {
                throw new ExceptionEngines($"Can not find with ID: {activityId}");
            }
            _context.Activities.Remove(activity);
            return await _context.SaveChangesAsync();
        }

        public async Task<PaginitionEngines<ActivityViewModel>> GetAll(ActivityFilter filter)
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
                .Take(filter.PageSize)
                .Select(x=> new ActivityViewModel()
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
            return result;
        }
    }
}
