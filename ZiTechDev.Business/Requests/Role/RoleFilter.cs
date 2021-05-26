using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZiTechDev.Business.Engines.Paginition;

namespace ZiTechDev.Business.Requests.Role
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
