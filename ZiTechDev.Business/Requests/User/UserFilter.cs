using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZiTechDev.Business.Engines.Paginition;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.Business.Requests.User
{
    public class UserFilter : PaginitionConfiguration
    {
        [Display(Name = "Mã")]
        public string Id { get; set; }

        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Tên hiển thị")]
        public string DisplayName { get; set; }

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime FromDOB { get; set; }

        [Display(Name = "Đến")]
        [DataType(DataType.Date)]
        public DateTime ToDOB { get; set; }

        [Display(Name = "Ngày đăng ký")]
        [DataType(DataType.Date)]
        public DateTime FromDOJ { get; set; }

        [Display(Name = "Đến")]
        [DataType(DataType.Date)]
        public DateTime ToDOJ { get; set; }

        [Display(Name = "Giới tính")]
        public int Gender { get; set; }
        public GenderSelector GenderSelectors
        {
            get
            {
                return new GenderSelector
                {
                    Items = new List<GenderItem>()
                    {
                        new GenderItem(-1, "Tất cả"),
                        new GenderItem(0, "Nam"),
                        new GenderItem(1, "Nữ"),
                        new GenderItem(2, "Giới tính khác")
                    }
                };
            }
        }

        [Display(Name = "Vai trò")]
        public List<RoleItem> Roles { get; set; } = new List<RoleItem>();

        public UserFilter()
        {
            Id = null;
            FullName = null;
            UserName = null;
            DisplayName = null;
            PhoneNumber = null;
            Email = null;
            FromDOB = DateTime.MinValue;
            ToDOB = DateTime.MaxValue;
            FromDOJ = DateTime.MinValue;
            ToDOJ = DateTime.MaxValue;
            Gender = -1; // All Genders
        }
    }
}
