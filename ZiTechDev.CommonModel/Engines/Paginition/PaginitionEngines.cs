using System.Collections.Generic;

namespace ZiTechDev.CommonModel.Engines.Paginition
{
    public class PaginitionEngines<T> : PaginitionConfiguration
    {
        public List<T> Item { get; set; }
    }
}
