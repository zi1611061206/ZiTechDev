using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Engines.Paginition
{
    public class PaginitionConfiguration : RequestBase
    {
        public int PageSize { get; set; }
        public int CurrentPageIndex { get; set; }
    }
}
