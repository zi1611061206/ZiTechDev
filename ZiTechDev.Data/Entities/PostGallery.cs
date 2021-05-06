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
        public byte[] EncodeString { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
