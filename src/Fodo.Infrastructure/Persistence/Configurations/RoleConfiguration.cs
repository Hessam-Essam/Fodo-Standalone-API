using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.ClientId)
            .HasColumnName("client_id");   

            builder.HasOne(x => x.Clients)
                .WithMany(c => c.Roles)         
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasMany(x => x.Permissions)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId);
        }
    }

}
