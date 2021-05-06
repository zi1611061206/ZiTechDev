using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Data.Entities
{
    public class PostTranslation
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string LanguageId { get; set; }
        public string SEODescription { get; set; }
        public string SEOTitle { get; set; }
        public string SEOAlias { get; set; }
        public string Content { get; set; }
        public Language Language { get; set; }
        public Post Post { get; set; }
    }
}
