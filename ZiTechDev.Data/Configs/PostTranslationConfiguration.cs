using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Data.Configs
{
    public class PostTranslationConfiguration : IEntityTypeConfiguration<PostTranslation>
    {
        public void Configure(EntityTypeBuilder<PostTranslation> builder)
        {
            builder.ToTable("PostTranslations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .UseIdentityColumn(1, 1);
            builder.Property(x => x.SEODescription)
                .IsRequired(true)
                .HasMaxLength(500)
                .IsUnicode(true);
            builder.Property(x => x.SEOTitle)
                .IsRequired(true)
                .HasMaxLength(50)
                .IsUnicode(true);
            builder.Property(x => x.SEOAlias)
                .IsRequired(false)
                .IsUnicode(false);
            builder.Property(x => x.Content)
                .IsRequired(true)
                .IsUnicode();
            builder.HasOne(l => l.Language)
                .WithMany(p => p.PostTranslations)
                .HasForeignKey(fk => fk.LanguageId);
            builder.HasOne(p => p.Post)
                .WithMany(pt => pt.PostTranslations)
                .HasForeignKey(fk => fk.PostId);
        }
    }
}
