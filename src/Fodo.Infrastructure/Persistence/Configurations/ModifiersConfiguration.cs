using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public sealed class ModifiersConfiguration : IEntityTypeConfiguration<Modifiers>
    {
        public void Configure(EntityTypeBuilder<Modifiers> b)
        {
            b.ToTable("Modifiers");

            b.HasKey(x => x.Id);

            b.Property(x => x.NameEn).HasMaxLength(200).IsRequired();
            b.Property(x => x.NameAr).HasMaxLength(200);

            b.Property(x => x.BasePrice)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            b.Property(x => x.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            b.HasOne(x => x.ModifiersGroup)
                .WithMany(x => x.Modifiers)
                .HasForeignKey(x => x.ModifierGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => new { x.ClientId, x.ModifierGroupId });
        }
    }
}
