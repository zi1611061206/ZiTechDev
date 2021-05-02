using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.Data.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModify { get; set; }
        public int UserId { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int SharedCount { get; set; }
        public PostStatus Status { get; set; }
        public int CategoryId { get; set; }
        public byte[] Thumbnail { get; set; }
    }
}
