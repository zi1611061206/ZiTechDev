using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.Business.Requests.User
{
    public class UserFilter : PaginitionConfiguration
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public DateTime FromDOB { get; set; }
        public DateTime ToDOB { get; set; }

        public UserFilter()
        {
            FullName = null;
            UserName = null;
            DisplayName = null;
            PhoneNumber = null;
            Email = null;
            Gender = -1; // All Gender
            FromDOB = DateTime.MinValue;
            ToDOB = DateTime.MaxValue;
        }
    }
}
