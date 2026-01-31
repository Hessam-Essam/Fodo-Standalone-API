using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public class ItemPricelistsConfig : IEntityTypeConfiguration<ItemPricelists>
    {
        public void Configure(EntityTypeBuilder<ItemPricelists> builder)
        {
            builder.ToTable("ItemPricelists");

            builder.HasKey(x => new { x.ItemId, x.PricelistId });

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(x => x.Items)
                .WithMany(i => i.ItemPricelists)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.PriceList)
                .WithMany(p => p.ItemPricelists)
                .HasForeignKey(x => x.PricelistId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
