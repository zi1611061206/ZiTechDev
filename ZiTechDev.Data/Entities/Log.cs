using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Data.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public DateTime ActionTime { get; set; }
        public int UserId { get; set; }
    }
}
