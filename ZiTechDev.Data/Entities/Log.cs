using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Data.Entities
{
    public class Log
    {
        public int ActivityId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ActionTime { get; set; }
        public Activity Activity { get; set; }
        public AppUser AppUser { get; set; }
    }
}
