using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Data.Configs
{
    public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
    {
        public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
        {
            builder.ToTable("CategoryTranslations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .UseIdentityColumn(1, 1);
            builder.Property(x => x.Name)
                .IsRequired(true)
                .HasMaxLength(50)
                .IsUnicode(true);
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
            builder.HasOne(l => l.Language)
                .WithMany(c => c.CategoryTranslations)
                .HasForeignKey(fk => fk.LanguageId);
            builder.HasOne(c => c.Category)
                .WithMany(ct => ct.CategoryTranslations)
                .HasForeignKey(fk => fk.CategoryId);
        }
    }
}
