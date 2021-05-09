using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Requests.User
{
    public class ChangePasswordRequest
    {
        public Guid Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}
