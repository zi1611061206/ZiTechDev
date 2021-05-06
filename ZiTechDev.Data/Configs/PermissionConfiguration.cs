using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.Data.Configs
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(x => new { x.RoleId, x.ActivityId});
            builder.HasOne(r => r.AppRole)
                .WithMany(p => p.Permissions)
                .HasForeignKey(fk => fk.RoleId);
            builder.HasOne(a => a.Activity)
                .WithMany(p => p.Permissions)
                .HasForeignKey(fk => fk.ActivityId);
        }
    }
}
