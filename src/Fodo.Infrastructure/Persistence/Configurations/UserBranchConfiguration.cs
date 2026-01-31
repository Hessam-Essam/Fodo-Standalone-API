using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public class UserBranchConfig : IEntityTypeConfiguration<UserBranches>
    {
        public void Configure(EntityTypeBuilder<UserBranches> builder)
        {
            builder.ToTable("UserBranches");
            builder.HasKey(x => x.UserBranchId);

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.BranchId).IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(u => u.UserBranches)
                .HasForeignKey(x => x.UserId)
                .HasPrincipalKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Branch)
                .WithMany(b => b.UserBranches)
                .HasForeignKey(x => x.BranchId)
                .HasPrincipalKey(b => b.BranchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }



}
