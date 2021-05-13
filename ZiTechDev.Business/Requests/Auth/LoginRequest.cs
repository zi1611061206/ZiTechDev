using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZiTechDev.Business.Requests.Auth
{
    public class LoginRequest
    {
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Ghi nhớ tôi")]
        public bool RememberMe { get; set; }
    }
}
