using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Data.Entities
{
    public class PostGallery
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string Path { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsThumbnail { get; set; }
        public double? FileSize { get; set; }
        public int? SortOrder { get; set; }
        public byte[] EncodeString { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
