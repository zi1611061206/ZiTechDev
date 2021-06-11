using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZiTechDev.CommonModel.Requests.Auth
{
    public class ResetPasswordRequest
    {
        [Display(Name = "Mã")]
        public Guid Id { get; set; }
        [Display(Name = "Mã thông báo bảo mật")]
        public string Token { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }
    }
}
