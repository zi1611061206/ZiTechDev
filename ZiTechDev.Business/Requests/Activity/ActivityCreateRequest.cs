using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZiTechDev.Business.Requests.Activity
{
    public class ActivityCreateRequest
    {
        [Display(Name = "Tên hành động")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Thuộc hàm")]
        public int FunctionId { get; set; }
    }
}
