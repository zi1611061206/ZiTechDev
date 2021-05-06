using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Data.Configs
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Logs");
            builder.HasKey(x => new { x.ActivityId, x.UserId });
            builder.Property(x => x.ActionTime)
                .HasDefaultValueSql("getdate()");
            builder.HasOne(a => a.Activity)
                .WithMany(l => l.Logs)
                .HasForeignKey(fk => fk.ActivityId);
            builder.HasOne(u => u.AppUser)
                .WithMany(l => l.Logs)
                .HasForeignKey(fk => fk.UserId);
        }
    }
}
