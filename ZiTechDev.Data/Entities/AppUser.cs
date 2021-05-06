using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? LastAccess { get; set; }
        public DateTime DateOfJoin { get; set; }

        public List<Log> Logs { get; set; }
        public List<Post> Posts { get; set; }
    }
}
