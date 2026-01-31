using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public class PriceListsConfig : IEntityTypeConfiguration<PriceLists>
    {
        public void Configure(EntityTypeBuilder<PriceLists> builder)
        {
            builder.ToTable("PriceLists");
            builder.HasKey(x => x.PriceListId);

            builder.Property(x => x.NameEn).HasMaxLength(200);
            builder.Property(x => x.NameAr).HasMaxLength(200);

            builder.Property(x => x.IsActive).IsRequired();

            builder.Property(x => x.ClientId)
                .HasColumnName("clientId")
                .IsRequired();

            builder.HasOne(x => x.Clients)
                .WithMany(c => c.PriceLists)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
