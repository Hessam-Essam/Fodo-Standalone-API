using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public class ItemsConfig : IEntityTypeConfiguration<Items>
    {
        public void Configure(EntityTypeBuilder<Items> builder)
        {
            builder.ToTable("Items");
            builder.HasKey(x => x.ItemId);

            builder.Property(x => x.NameEn).HasMaxLength(200);
            builder.Property(x => x.NameAr).HasMaxLength(200);

            builder.Property(x => x.ItemType).IsRequired();

            builder.Property(x => x.BasePrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.IsActive).IsRequired();

            builder.Property(x => x.ClientId)
                .HasColumnName("clientId")
                .IsRequired();

            builder.HasOne(x => x.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TaxRule)
                .WithMany(t => t.Items)
                .HasForeignKey(x => x.TaxRuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Clients)
                .WithMany(c => c.Items)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
