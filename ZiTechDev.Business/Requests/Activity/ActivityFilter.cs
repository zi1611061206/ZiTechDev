using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Engines.Paginition;

namespace ZiTechDev.Business.Requests.Activity
{
    public class ActivityFilter : PaginitionConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> FunctionIds { get; set; }

        public ActivityFilter()
        {
            Id = 0;
            Name = null;
            FunctionIds = null;
        }
    }
}
