using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZiTechDev.CommonModel.Requests.CommonItems;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.CommonModel.Requests.Profile
{
    public class ProfileEditRequest
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
        public List<GenderItem> Genders { get; set; } = new List<GenderItem>()
                    {
                        new GenderItem(0, "Nam"),
                        new GenderItem(1, "Nữ"),
                        new GenderItem(2, "Giới tính khác")
                    };
    }
}
