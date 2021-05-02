using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Data.Entities
{
    public class Permission
    {
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
        public int ActionId { get; set; }
    }
}
