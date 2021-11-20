using System;
using System.ComponentModel.DataAnnotations;

namespace ZiTechDev.CommonModel.Requests.Role
{
    public class RoleUpdateRequest
    {
        [Display(Name = "Mã")]
        public Guid Id { get; set; }

        [Display(Name = "Tên vai trò")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }
}
