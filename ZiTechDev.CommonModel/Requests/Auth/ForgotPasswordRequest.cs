using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZiTechDev.CommonModel.Requests.Auth
{
    public class ForgotPasswordRequest
    {
        [Display(Name = "Email đăng ký của bạn")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
