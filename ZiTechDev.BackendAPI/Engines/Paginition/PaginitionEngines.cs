using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Engines.Paginition
{
    public class PaginitionEngines<T> : PaginitionConfiguration
    {
        public List<T> Item { get; set; }
    }
}
