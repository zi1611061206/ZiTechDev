using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.Business.Requests.User
{
    public class UserUpdateRequest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderType Gender { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
