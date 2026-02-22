using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public sealed class OrderItemModifierConfiguration : IEntityTypeConfiguration<OrderItemModifier>
    {
        public void Configure(EntityTypeBuilder<OrderItemModifier> b)
        {
            b.ToTable("OrderItemModifier"); // or "OrderItemModifiers" (match your DB!)
            b.HasKey(x => x.OrderItemModifierId);

            b.Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            b.HasOne(x => x.OrderItems)
            .WithMany(x => x.Modifiers)
            .HasForeignKey(x => x.OrderItemId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
