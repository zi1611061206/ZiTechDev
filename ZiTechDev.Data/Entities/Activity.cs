using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Data.Entities
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int FunctionId { get; set; }
        public Function Function { get; set; }
        public List<Log> Logs { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
