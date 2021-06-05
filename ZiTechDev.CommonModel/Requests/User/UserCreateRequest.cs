using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.CommonModel.Requests.User
{
    public class UserCreateRequest
    {
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
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Mật khẩu xác nhận")]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Giới tính")]
        public GenderType Gender { get; set; } = 0;
        public List<GenderItem> Genders { get; set; } = new List<GenderItem>()
                    {
                        new GenderItem(0, "Nam"),
                        new GenderItem(1, "Nữ"),
                        new GenderItem(2, "Giới tính khác")
                    };

        [Display(Name = "Vai trò")]
        public List<RoleItem> Roles { get; set; } = new List<RoleItem>();
    }
}
