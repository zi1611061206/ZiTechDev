using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Requests.Function
{
    public class FunctionCreateRequest
    {
        public string Name { get; set; }
        public string Desciption { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
    }
}
