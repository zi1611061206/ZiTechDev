using System;

namespace ZiTechDev.CommonModel.Requests.User
{
    public class RoleItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}
