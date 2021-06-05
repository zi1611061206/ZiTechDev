using System;
using System.ComponentModel.DataAnnotations;

namespace ZiTechDev.CommonModel.Requests.Role
{
    public class RoleViewModel
    {
        [Display(Name = "Mã")]
        public Guid Id { get; set; }

        [Display(Name = "Tên vai trò")]
        public string Name { get; set; }

        [Display(Name = "Tên chuẩn hóa")]
        public string NormalizedName { get; set; }

        [Display(Name = "Tem đồng bộ")]
        public string ConcurrencyStamp { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }
}
