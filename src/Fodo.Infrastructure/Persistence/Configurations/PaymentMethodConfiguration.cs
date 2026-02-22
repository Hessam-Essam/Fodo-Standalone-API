using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public sealed class PaymentMethodConfiguration : IEntityTypeConfiguration<Payment_Methods>
    {
        public void Configure(EntityTypeBuilder<Payment_Methods> b)
        {
            b.ToTable("Payment_methods", "dbo");

            b.HasKey(x => x.PaymentMethodId);

            b.Property(x => x.PaymentMethodId)
                .HasColumnName("payment_method_id");

            b.Property(x => x.ClientId)
                .HasColumnName("client_id")
                .IsRequired();

            b.Property(x => x.MethodName)
                .HasColumnName("method_name")
                .HasMaxLength(150)
                .IsRequired();

            b.Property(x => x.MethodType)
                .HasColumnName("method_type")
                .HasMaxLength(50)
                .IsRequired();

            b.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.HasIndex(x => new { x.ClientId, x.IsActive });
        }
    }
}
