using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public class ItemPricelistConfig : IEntityTypeConfiguration<ItemPricelists>
    {
        public void Configure(EntityTypeBuilder<ItemPricelists> b)
        {
            b.ToTable("ItemPricelists");
            b.HasKey(x => new { x.ItemId, x.PricelistId });

            b.HasOne(x => x.Items).WithMany(i => i.ItemPricelists).HasForeignKey(x => x.ItemId);
            b.HasOne(x => x.PriceList).WithMany(p => p.ItemPricelists).HasForeignKey(x => x.PricelistId);
        }
    }
}
