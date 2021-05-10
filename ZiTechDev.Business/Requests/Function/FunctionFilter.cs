using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Engines.Paginition;

namespace ZiTechDev.Business.Requests.Function
{
    public class FunctionFilter : PaginitionConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public FunctionFilter()
        {
            CurrentPageIndex = 1;
            PageSize = 10;
            Id = 0;
            Name = null;
            ParentId = -1;
        }
    }
}
