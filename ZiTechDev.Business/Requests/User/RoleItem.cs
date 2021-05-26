using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Requests.User
{
    public class RoleItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}
