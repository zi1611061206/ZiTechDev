using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Entities;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.Data.Configs
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("getdate()");
            builder.Property(x => x.ViewCount)
                .HasDefaultValue(0);
            builder.Property(x => x.LikeCount)
                .HasDefaultValue(0);
            builder.Property(x => x.SharedCount)
                .HasDefaultValue(0);
            builder.Property(x => x.Status)
                .HasDefaultValue(PostStatus.Pending);
            builder.Property(x => x.Thumbnail)
                .IsRequired(false);
        }
    }
}
