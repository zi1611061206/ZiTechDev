using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Data.Configs
{
    public class FunctionConfiguration : IEntityTypeConfiguration<Function>
    {
        public void Configure(EntityTypeBuilder<Function> builder)
        {
            builder.ToTable("Functions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .IsRequired(true)
                .HasMaxLength(50)
                .IsUnicode(true);
            builder.Property(x => x.Desciption)
                .IsRequired(false)
                .HasMaxLength(500)
                .IsUnicode(true);
            builder.Property(x => x.Url)
                .IsRequired(false)
                .IsUnicode(false);
            builder.Property(x => x.ParentId)
                .IsRequired(false);
        }
    }
}
