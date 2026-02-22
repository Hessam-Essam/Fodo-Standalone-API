using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> b)
        {
            b.ToTable("OrderItems");
            b.HasKey(x => x.OrderItemId);

            b.Property(x => x.Price).HasColumnType("decimal(18,2)");

            b.HasMany(x => x.Modifiers)
                .WithOne(x => x.OrderItems)
                .HasForeignKey(x => x.OrderItemId);

            b.Property(x => x.Note).HasMaxLength(250);
        }
    }
}
