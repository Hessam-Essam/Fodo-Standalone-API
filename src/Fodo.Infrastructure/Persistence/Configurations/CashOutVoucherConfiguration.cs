using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public sealed class CashOutVoucherConfiguration : IEntityTypeConfiguration<CashOutVoucher>
    {
        public void Configure(EntityTypeBuilder<CashOutVoucher> b)
        {
            b.ToTable("CashOutVouchers", "dbo");

            // PK
            b.HasKey(x => x.CashOutVoucherId);

            b.Property(x => x.CashOutVoucherId)
                .HasColumnName("CashOutVoucherId");

            // Required fields
            b.Property(x => x.BranchId).IsRequired();
            b.Property(x => x.ShiftId).IsRequired();

            b.Property(x => x.Amount)
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

            // Indexes for reporting
            b.HasIndex(x => new { x.BranchId, x.ShiftId, x.CreatedAt });
            b.HasIndex(x => x.DeviceId);
            b.HasIndex(x => x.RowGuid).IsUnique();
        }
    }
}
