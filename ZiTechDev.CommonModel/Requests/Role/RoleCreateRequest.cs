using System.ComponentModel.DataAnnotations;

namespace ZiTechDev.CommonModel.Requests.Role
{
    public class RoleCreateRequest
    {
        [Display(Name = "Tên vai trò")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }
}
