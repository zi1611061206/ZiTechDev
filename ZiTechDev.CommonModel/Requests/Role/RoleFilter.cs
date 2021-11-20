using System.ComponentModel.DataAnnotations;
using ZiTechDev.CommonModel.Engines.Paginition;

namespace ZiTechDev.CommonModel.Requests.Role
{
    public class RoleFilter : PaginitionConfiguration
    {
        [Display(Name = "Mã")]
        public string Id { get; set; }

        [Display(Name = "Tên vai trò")]
        public string Name { get; set; }

        public RoleFilter()
        {
            Id = null;
            Name = null;
        }
    }
}
