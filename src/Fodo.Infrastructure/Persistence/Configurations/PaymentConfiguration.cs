using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payments>
    {
        public void Configure(EntityTypeBuilder<Payments> b)
        {
            b.ToTable("Payments", "dbo");

            b.HasKey(x => x.PaymentId);

            b.Property(x => x.PaymentId)
                .HasColumnName("PaymentId");

            b.Property(x => x.OrderId)
                .HasColumnName("OrderId")
                .IsRequired();

            b.Property(x => x.Method)
                .HasColumnName("Method")
                .HasMaxLength(100);

            b.Property(x => x.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            b.Property(x => x.RowGuid)
                .HasColumnName("RowGuid")
                .IsRequired();

            b.Property(x => x.RowVersion)
                .HasColumnName("RowVersion")
                .IsRowVersion()
                .IsConcurrencyToken();

            b.Property(x => x.DeviceId)
                .HasColumnName("DeviceId")
                .HasMaxLength(100)
                .IsRequired();

            b.Property(x => x.SyncedAt)
                .HasColumnName("SyncedAt");

            b.Property(x => x.IsSynced)
                .HasColumnName("IsSynced")
                .IsRequired();

            b.Property(x => x.PaymentMethodId)
                .HasColumnName("Payment_method_id");

            // FK to Payment_methods (NO ACTION to avoid cascade path issues)
            b.HasOne(x => x.PaymentMethod)
                .WithMany(m => m.Payments)
                .HasForeignKey(x => x.PaymentMethodId)
                .OnDelete(DeleteBehavior.NoAction);

            b.HasIndex(x => x.OrderId);
            b.HasIndex(x => x.PaymentMethodId);
            b.HasIndex(x => new { x.DeviceId, x.CreatedAt });
        }
    }
}
