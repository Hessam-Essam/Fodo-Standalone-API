using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Persistence
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<UserBranches> UserBranches => Set<UserBranches>();
        public DbSet<Clients> Clients => Set<Clients>();
        public DbSet<Branches> Branches => Set<Branches>();
        public DbSet<Devices> Devices => Set<Devices>();
        public DbSet<ItemPricelists> ItemPricelists => Set<ItemPricelists>();
        public DbSet<Items> Items => Set<Items>();
        public DbSet<Categories> Categories => Set<Categories>();
        public DbSet<CategoryBranches> CategoryBranches => Set<CategoryBranches>();
        public DbSet<PriceLists> PriceLists => Set<PriceLists>();
        public DbSet<TaxRules> TaxRules => Set<TaxRules>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(IdentityDbContext).Assembly);

            modelBuilder.Entity<Role>().HasOne(r => r.Clients).WithMany(c => c.Roles).HasForeignKey(r => r.ClientId)
                                       .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public sealed class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<IdentityDbContext>()
                .UseSqlServer("Server=.;Database=Fodo_DB;Integrated Security=True;MultipleActiveResultSets=True;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            return new IdentityDbContext(options);
        }
    }

}
