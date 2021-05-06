using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Data.Configs
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Users");
            builder.Property(x => x.FirstName)
                .IsRequired(true)
                .HasMaxLength(20)
                .IsUnicode(true);
            builder.Property(x => x.MiddleName)
                .IsRequired(false)
                .HasMaxLength(20)
                .IsUnicode(true);
            builder.Property(x => x.LastName)
                .IsRequired(true)
                .HasMaxLength(20)
                .IsUnicode(true);
            builder.Property(x => x.DisplayName)
                .IsRequired(false)
                .HasMaxLength(50)
                .IsUnicode(true);
            builder.Property(x => x.DateOfBirth)
                .IsRequired(true);
            builder.Property(x => x.LastAccess)
                .IsRequired(false);
            builder.Property(x => x.DateOfJoin)
                .HasDefaultValueSql("getdate()");
        }
    }
}
