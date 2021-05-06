using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.Data.Entities
{
    public class Setting
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public ConfigType Type { get; set; }
    }
}
