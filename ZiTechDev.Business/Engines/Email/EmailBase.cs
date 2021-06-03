using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Engines.Email
{
    public class EmailBase
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public EmailBase(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}
