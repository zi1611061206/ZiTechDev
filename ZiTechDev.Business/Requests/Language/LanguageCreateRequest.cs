using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Requests.Language
{
    public class LanguageCreateRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
