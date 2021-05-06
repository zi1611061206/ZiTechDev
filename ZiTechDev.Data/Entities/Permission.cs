using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Data.Entities
{
    public class Permission
    {
        public Guid RoleId { get; set; }
        public int ActivityId { get; set; }

        public AppRole AppRole { get; set; }
        public Activity Activity { get; set; }
    }
}
