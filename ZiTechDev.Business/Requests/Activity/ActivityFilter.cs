using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZiTechDev.Business.Engines.Paginition;

namespace ZiTechDev.Business.Requests.Activity
{
    public class ActivityFilter : PaginitionConfiguration
    {
        [Display(Name = "Mã")]
        public int Id { get; set; }
        [Display(Name = "Tên hành động")]
        public string Name { get; set; }
        [Display(Name = "Thuộc hàm")]
        public List<int> FunctionIds { get; set; }

        public ActivityFilter()
        {
            CurrentPageIndex = 1;
            PageSize = 10;
            Id = 0;
            Name = null;
            FunctionIds = new List<int>();
        }
    }
}
