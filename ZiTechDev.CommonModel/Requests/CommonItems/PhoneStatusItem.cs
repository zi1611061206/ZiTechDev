using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.CommonModel.Requests.CommonItems
{
    public class PhoneStatusItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PhoneStatusItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
