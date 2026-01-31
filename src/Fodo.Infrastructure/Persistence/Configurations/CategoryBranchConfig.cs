using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public class CategoryBranchConfig : IEntityTypeConfiguration<CategoryBranches>
    {
        public void Configure(EntityTypeBuilder<CategoryBranches> b)
        {
            b.ToTable("CategoryBranches");
            b.HasKey(x => new { x.CategoryId, x.BranchId });

            b.HasOne(x => x.Category).WithMany(c => c.CategoryBranches).HasForeignKey(x => x.CategoryId);
            b.HasOne(x => x.Branches).WithMany(br => br.CategoryBranches).HasForeignKey(x => x.BranchId);
        }
    }
}
