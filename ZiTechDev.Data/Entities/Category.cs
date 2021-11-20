using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? SortOrder { get; set; }
        public List<Post> Posts { get; set; }
        public List<CategoryTranslation> CategoryTranslations { get; set; }
    }
}
