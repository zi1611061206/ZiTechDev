using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Requests.User
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
