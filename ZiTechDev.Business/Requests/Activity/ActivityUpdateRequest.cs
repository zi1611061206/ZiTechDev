using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZiTechDev.Business.Requests.Activity
{
    public class ActivityUpdateRequest
    {
        [Display(Name = "Mã")]
        public int Id { get; set; }
        [Display(Name = "Tên hành động")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Thuộc loại")]
        public int FunctionId { get; set; }
    }
}
