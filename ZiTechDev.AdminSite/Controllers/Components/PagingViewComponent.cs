using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.Paginition;

namespace ZiTechDev.AdminSite.Controllers.Components
{
    public class PagingViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PaginitionConfiguration config)
        {
            return Task.FromResult((IViewComponentResult)View("Default", config));
        } 
    }
}
