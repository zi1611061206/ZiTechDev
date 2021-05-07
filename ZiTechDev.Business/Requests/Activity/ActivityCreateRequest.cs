using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Requests.Activity
{
    public class ActivityCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int FunctionId { get; set; }
    }
}
