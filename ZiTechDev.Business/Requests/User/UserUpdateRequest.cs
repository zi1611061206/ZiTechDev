﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZiTechDev.Business.Engines;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.Business.Requests.User
{
    public class UserUpdateRequest
    {
        [Display(Name = "Mã")]
        public Guid Id { get; set; }
        [Display(Name = "Họ")]
        public string FirstName { get; set; }
        [Display(Name = "Tên đệm")]
        public string MiddleName { get; set; }
        [Display(Name = "Tên")]
        public string LastName { get; set; }
        [Display(Name = "Tên hiển thị")]
        public string DisplayName { get; set; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Giới tính")]
        public GenderType Gender { get; set; }

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
