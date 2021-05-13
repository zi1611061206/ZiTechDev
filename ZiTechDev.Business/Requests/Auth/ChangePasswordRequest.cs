﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZiTechDev.Business.Requests.Auth
{
    public class ChangePasswordRequest
    {
        [Display(Name = "Mã")]
        public Guid Id { get; set; }
        [Display(Name = "Mật khẩu cũ")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }
    }
}
