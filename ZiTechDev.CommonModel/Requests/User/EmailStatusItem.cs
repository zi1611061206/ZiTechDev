using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.CommonModel.Requests.User
{
    public class EmailStatusItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public EmailStatusItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
