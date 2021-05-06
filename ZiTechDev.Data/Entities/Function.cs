using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Data.Entities
{
    public class Function
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
