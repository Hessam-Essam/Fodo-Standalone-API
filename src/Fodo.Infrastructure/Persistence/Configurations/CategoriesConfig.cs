using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    using Fodo.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CategoriesConfig : IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(x => x.CategoryId);

            builder.Property(x => x.CategoryId)
                .HasColumnName("CategoryId");

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.ClientId)
                .HasColumnName("client_id")
                .IsRequired();

            builder.HasOne(x => x.Clients)
                .WithMany(c => c.Categories)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
