using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Data.Configs
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Time)
                .HasDefaultValueSql("getdate()");
            builder.Property(x => x.Content)
                .IsRequired(true)
                .HasMaxLength(500)
                .IsUnicode(true);
            builder.Property(x => x.LikeCount)
                .HasDefaultValue(0);
            builder.Property(x => x.ParentId)
                .IsRequired(false);
        }
    }
}
