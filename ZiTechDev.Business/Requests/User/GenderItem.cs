using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Requests.User
{
    public class GenderItem
    {
        public int Id { get; set; }
        public string GenderName { get; set; }
        public GenderItem(int id, string genderName)
        {
            Id = id;
            GenderName = genderName;
        }
    }
}
