using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZiTechDev.CommonModel.Engines.Paginition;

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
        [DataType(DataType.EmailAddress)]
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

        [Display(Name = "Giới tính")]
        public int Gender { get; set; } = -1;
        public List<GenderItem> Genders { get; set; } = new List<GenderItem>()
                    {
                        new GenderItem(-1, "Tất cả"),
                        new GenderItem(0, "Nam"),
                        new GenderItem(1, "Nữ"),
                        new GenderItem(2, "Giới tính khác")
                    };

        [Display(Name = "Vai trò")]
        public List<RoleItem> Roles { get; set; } = new List<RoleItem>();

    }
}
