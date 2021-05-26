using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.Business.Requests.User
{
    public class UserViewModel
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
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Lần truy cập gần nhất")]
        public DateTime? LastAccess { get; set; }

        [Display(Name = "Ngày đăng ký")]
        public DateTime DateOfJoin { get; set; }

        [Display(Name = "Giới tính")]
        public GenderType Gender { get; set; }

        [Display(Name = "Thời gian khóa")]
        public DateTimeOffset? LockoutEnd { get; set; }

        [Display(Name = "Xác thực 2 bước")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = "Đã xác thực số điện thoại")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Tem đồng bộ")]
        public string ConcurrencyStamp { get; set; }

        [Display(Name = "Tem bảo mật")]
        public string SecurityStamp { get; set; }

        //public string PasswordHash { get; set; }

        [Display(Name = "Đã xác thực Email")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Email chuẩn hóa")]
        public string NormalizedEmail { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Tên đăng nhập chuẩn hóa")]
        public string NormalizedUserName { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Mã")]
        public Guid Id { get; set; }

        [Display(Name = "Có thể khóa")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Số lần truy cập thất bại")]
        public int AccessFailedCount { get; set; }

        [Display(Name = "Vai trò")]
        public IList<string> Roles { get; set; }
    }
}
