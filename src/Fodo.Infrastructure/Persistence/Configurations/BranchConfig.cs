using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence.Configurations
{
    public class BranchConfig : IEntityTypeConfiguration<Branches>
    {
        public void Configure(EntityTypeBuilder<Branches> b)
        {
            b.ToTable("Branches");
            b.HasKey(x => x.BranchId);

            b.Property(x => x.ClientId).HasColumnName("client_id");
        }
    }
}
