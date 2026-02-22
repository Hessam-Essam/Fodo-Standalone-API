using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public sealed class CashInVoucherConfiguration : IEntityTypeConfiguration<CashInVoucher>
    {
        public void Configure(EntityTypeBuilder<CashInVoucher> b)
        {
            b.ToTable("CashInVouchers");

            b.HasKey(x => x.CashInVoucherId);

            b.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            b.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            b.Property(x => x.DeviceId)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
