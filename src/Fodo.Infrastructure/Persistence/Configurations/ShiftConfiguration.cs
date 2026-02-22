using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public sealed class ShiftConfiguration : IEntityTypeConfiguration<Shifts>
    {
        public void Configure(EntityTypeBuilder<Shifts> b)
        {
            b.ToTable("Shifts");

            b.HasKey(x => x.ShiftId);

            b.Property(x => x.OpeningCash)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            b.Property(x => x.ClosingCash)
                .HasColumnType("decimal(18,2)");

            b.Property(x => x.StartTime).IsRequired();
            b.Property(x => x.EndTime);

            b.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            b.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            b.HasIndex(x => new { x.BranchId, x.StartTime });
        }
    }

}
