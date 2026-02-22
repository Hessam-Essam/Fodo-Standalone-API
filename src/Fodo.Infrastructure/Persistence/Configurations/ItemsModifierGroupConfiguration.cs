using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public class ItemsModifierGroupConfiguration : IEntityTypeConfiguration<ItemsModifierGroup>
    {
        public void Configure(EntityTypeBuilder<ItemsModifierGroup> b)
        {
            b.ToTable("ItemsModifierGroups");

            b.HasKey(x => x.Id);

            b.HasOne(x => x.Item)
                .WithMany()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.ModifiersGroup)
                .WithMany(x => x.ItemsModifierGroup)
                .HasForeignKey(x => x.ModifierGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicates
            b.HasIndex(x => new { x.ItemId, x.ModifierGroupId })
                .IsUnique();

            b.HasIndex(x => new { x.ClientId, x.ItemId });
        }
    }
}
