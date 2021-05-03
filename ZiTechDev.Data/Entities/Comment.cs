using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }
        public int? ParentId { get; set; }
        public int LikeCount { get; set; }
        public DateTime? LastModify { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
