using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.Paginition;

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
