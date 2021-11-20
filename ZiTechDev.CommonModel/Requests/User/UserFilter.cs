using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZiTechDev.CommonModel.Engines.Paginition;
using ZiTechDev.CommonModel.Requests.CommonItems;

namespace ZiTechDev.CommonModel.Requests.User
{
    public class UserFilter : PaginitionConfiguration
    {
        public string CurrentUserId { get; set; } = null;


        [Display(Name = "Mã")]
        public string Id { get; set; } = null;

        [Display(Name = "Họ tên")]
        public string FullName { get; set; } = null;

        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; } = null;

        [Display(Name = "Tên hiển thị")]
        public string DisplayName { get; set; } = null;

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null;

        [Display(Name = "Email")]
        public string Email { get; set; } = null;

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime FromDOB { get; set; } = DateTime.MinValue;

        [Display(Name = "Đến")]
        [DataType(DataType.Date)]
        public DateTime ToDOB { get; set; } = DateTime.MaxValue;

        [Display(Name = "Ngày đăng ký")]
        [DataType(DataType.Date)]
        public DateTime FromDOJ { get; set; } = DateTime.MinValue;

        [Display(Name = "Đến")]
        [DataType(DataType.Date)]
        public DateTime ToDOJ { get; set; } = DateTime.MaxValue;

        [Display(Name = "Vai trò")]
        public List<RoleItem> Roles { get; set; } = new List<RoleItem>();

        [Display(Name = "Giới tính")]
        public int Gender { get; set; } = -1;
        public List<GenderItem> Genders { get; set; }

        [Display(Name = "Xác thực email")]
        public int EmailConfirmed { get; set; } = -1;
        public List<EmailStatusItem> EmailStatus { get; set; }

        [Display(Name = "Xác thực số điện thoại")]
        public int PhoneNumberConfirmed { get; set; } = -1;
        public List<PhoneStatusItem> PhoneStatus { get; set; }

        [Display(Name = "Xác minh 2 bước")]
        public int TwoFactorEnabled { get; set; } = -1;
        public List<TwoFactorStatusItem> TwoFactorStatus { get; set; }

        public UserFilter()
        {
            Genders = new List<GenderItem>()
                    {
                        new GenderItem(-1, "Tất cả"),
                        new GenderItem(0, "Nam"),
                        new GenderItem(1, "Nữ"),
                        new GenderItem(2, "Giới tính khác")
                    };
            EmailStatus = new List<EmailStatusItem>()
                    {
                        new EmailStatusItem(-1, "Tất cả"),
                        new EmailStatusItem(0, "Chưa xác thực"),
                        new EmailStatusItem(1, "Đã xác thực")
                    };
            PhoneStatus = new List<PhoneStatusItem>()
                    {
                        new PhoneStatusItem(-1, "Tất cả"),
                        new PhoneStatusItem(0, "Chưa xác thực"),
                        new PhoneStatusItem(1, "Đã xác thực")
                    };
            TwoFactorStatus = new List<TwoFactorStatusItem>()
                    {
                        new TwoFactorStatusItem(-1, "Tất cả"),
                        new TwoFactorStatusItem(0, "Chưa thiết lập"),
                        new TwoFactorStatusItem(1, "Đã thiết lập")
                    };
        }
    }
}
