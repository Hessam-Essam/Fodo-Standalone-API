using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    
    public sealed class OrderRefundConfiguration : IEntityTypeConfiguration<OrderRefund>
    {
        public void Configure(EntityTypeBuilder<OrderRefund> b)
        {
            b.ToTable("OrderRefunds", "dbo");

            // PK
            b.HasKey(x => x.RefundId);

            b.Property(x => x.RefundId)
                .HasColumnName("RefundId");

            // Required fields
            b.Property(x => x.OrderId)
                .IsRequired();

            b.Property(x => x.BranchId)
                .IsRequired();

            b.Property(x => x.ShiftId)
                .IsRequired();

            b.Property(x => x.RefundAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            b.Property(x => x.DeviceId)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .IsRequired();

            // Optional
            b.Property(x => x.UserId);

            b.Property(x => x.Reason)
                .HasMaxLength(250);

            // Sync columns
            b.Property(x => x.RowGuid)
                .IsRequired();

            b.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            b.Property(x => x.SyncedAt);
            b.Property(x => x.IsSynced).IsRequired();

            // We keep Refund.OrderId as value-only by default (no FK to Orders)
            // Because Orders might be in another context or to avoid cascade paths.
            // If you want FK, enable it with NoAction:
            //
            // b.HasOne<Order>()
            //   .WithMany()
            //   .HasForeignKey(x => x.OrderId)
            //   .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            b.HasIndex(x => new { x.BranchId, x.ShiftId, x.CreatedAt });
            b.HasIndex(x => x.OrderId);
            b.HasIndex(x => x.DeviceId);
            b.HasIndex(x => x.RowGuid).IsUnique();
        }
    }
}
