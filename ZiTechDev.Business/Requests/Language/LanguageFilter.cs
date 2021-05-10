using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Engines.Paginition;

namespace ZiTechDev.Business.Requests.Language
{
    public class LanguageFilter : PaginitionConfiguration
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int IsDefault { get; set; }

        public LanguageFilter()
        {
            CurrentPageIndex = 1;
            PageSize = 10;
            Id = null;
            Name = null;
            IsDefault = -1;
        }
    }
}
