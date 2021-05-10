using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Business.Engines.Paginition;

namespace ZiTechDev.Business.Requests.Setting
{
    public class SettingFilter : PaginitionConfiguration
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int Type { get; set; }

        public SettingFilter()
        {
            CurrentPageIndex = 1;
            PageSize = 10;
            Key = null;
            Value = null;
            Type = -1;
        }
    }
}
