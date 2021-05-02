using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.Data.Entities
{
    public class Setting
    {
        public string key { get; set; }
        public string value { get; set; }
        public ConfigType Type { get; set; }
    }
}
