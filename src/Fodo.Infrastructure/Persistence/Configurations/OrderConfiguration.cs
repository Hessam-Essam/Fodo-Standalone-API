using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public sealed class OrderConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> b)
        {
            b.ToTable("Orders");
            b.HasKey(x => x.OrderId);

            b.Property(x => x.OrderNumber).HasMaxLength(50).IsRequired();
            b.Property(x => x.Status).HasMaxLength(30).IsRequired();

            b.Property(x => x.SubTotal).HasColumnType("decimal(18,2)");
            b.Property(x => x.TaxAmount).HasColumnType("decimal(18,2)");
            b.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");

            b.Property(x => x.DeviceId).HasMaxLength(100).IsRequired();
            b.Property(x => x.RowGuid).IsRequired();
            b.Property(x => x.Note).HasMaxLength(250);
            b.HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);
        }
    }
}
