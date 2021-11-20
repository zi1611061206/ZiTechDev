using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Data.Configurations
{
    public class PostGalleryConfiguration : IEntityTypeConfiguration<PostGallery>
    {
        public void Configure(EntityTypeBuilder<PostGallery> builder)
        {
            builder.ToTable("PostGalleries");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .UseIdentityColumn(1, 1);
            builder.Property(x => x.Caption)
                .IsRequired(true)
                .HasMaxLength(500)
                .IsUnicode(true);
            builder.Property(x => x.Path)
                .IsRequired(false)
                .IsUnicode(false);
            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("getdate()");
            builder.Property(x => x.IsThumbnail)
                .HasDefaultValue(false);
            builder.Property(x => x.FileSize)
                .HasDefaultValue(0.0);
            builder.Property(x => x.SortOrder)
                .HasDefaultValue(0);
            builder.Property(x => x.EncodeString)
                .IsRequired(false);
            builder.HasOne(p => p.Post)
                .WithMany(pg => pg.PostGalleries)
                .HasForeignKey(fk => fk.PostId);
        }
    }
}
