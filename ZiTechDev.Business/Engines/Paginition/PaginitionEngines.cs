using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Engines.Paginition
{
    public class PaginitionEngines<T>
    {
        public List<T> Item { get; set; }

        public int TotalRecord { get; set; }
    }
}
