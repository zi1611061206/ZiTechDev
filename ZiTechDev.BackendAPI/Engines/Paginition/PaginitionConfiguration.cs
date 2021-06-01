using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Engines.Paginition
{
    public class PaginitionConfiguration
    {
        public string Keyword { get; set; }
        public int PageSize { get; set; }
        public int CurrentPageIndex { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages
        {
            get
            { 
                return (int)Math.Ceiling((double)TotalRecords / PageSize);
            }
        }
        public int TotalRecordsLastPage
        {
            get
            {
                return TotalRecords - PageSize * (TotalPages - 1);
            }
        }

        public PaginitionConfiguration(int pageSize = 10, int currentPageIndex = 1)
        {
            Keyword = "";
            PageSize = pageSize;
            CurrentPageIndex = currentPageIndex;
        }
    }
}
