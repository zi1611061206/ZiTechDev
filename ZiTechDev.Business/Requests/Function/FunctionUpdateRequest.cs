using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Requests.Function
{
    public class FunctionUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desciption { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
    }
}
