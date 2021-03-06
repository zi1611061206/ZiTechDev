using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Data.Entities
{
    public class CategoryTranslation
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string SEODescription { get; set; }
        public string SEOTitle { get; set; }
        public string SEOAlias { get; set; }
        public string LanguageId { get; set; }
        public Language Language { get; set; }
        public Category Category { get; set; }
    }
}
