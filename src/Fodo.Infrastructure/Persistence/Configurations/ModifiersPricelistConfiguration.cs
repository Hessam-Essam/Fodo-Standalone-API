using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public class ModifiersPricelistConfiguration : IEntityTypeConfiguration<ModifiersPricelist>
    {
        public void Configure(EntityTypeBuilder<ModifiersPricelist> b)
        {
            b.ToTable("ModifierPricelists");

            b.HasKey(x => x.Id);

            b.Property(x => x.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            b.HasOne(x => x.Modifiers)
                .WithMany(x => x.ModifiersPricelist)
                .HasForeignKey(x => x.ModifierId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.PriceLists)
                .WithMany()
                .HasForeignKey(x => x.PriceListId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prevent duplicate price entries
            b.HasIndex(x => new { x.ModifierId, x.PriceListId })
                .IsUnique();
        }
    }
}
