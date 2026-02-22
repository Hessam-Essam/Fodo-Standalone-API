using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public sealed class ModifiersGroupConfiguration : IEntityTypeConfiguration<ModifiersGroup>
    {
        public void Configure(EntityTypeBuilder<ModifiersGroup> b)
        {
            b.ToTable("ModifierGroups");

            b.HasKey(x => x.Id);

            b.Property(x => x.NameEn)
                .HasMaxLength(200)
                .IsRequired();

            b.Property(x => x.NameAr)
                .HasMaxLength(200);

            b.Property(x => x.MinSelect)
                .IsRequired()
                .HasDefaultValue(0);

            b.Property(x => x.MaxSelect)
                .IsRequired()
                .HasDefaultValue(1);

            b.Property(x => x.IsRequired)
                .IsRequired()
                .HasDefaultValue(false);

            b.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            b.Property(x => x.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        }
    }
}
