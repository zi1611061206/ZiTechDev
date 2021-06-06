using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.CommonModel.Requests.User
{
    public class TwoFactorStatusItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TwoFactorStatusItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
