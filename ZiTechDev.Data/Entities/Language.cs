using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.Data.Entities
{
    public class Language
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Default IsDefault { get; set; }
        public List<CategoryTranslation> CategoryTranslations { get; set; }
        public List<PostTranslation> PostTranslations { get; set; }
    }
}
