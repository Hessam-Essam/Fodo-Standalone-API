using Fodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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
        public DbSet<ItemsModifierGroup> ItemsModifierGroups => Set<ItemsModifierGroup>();
        public DbSet<Modifiers> Modifiers => Set<Modifiers>();
        public DbSet<ModifiersGroup> ModifiersGroup => Set<ModifiersGroup>();
        public DbSet<ModifiersPricelist> ModifiersPricelist => Set<ModifiersPricelist>();
        public DbSet<Orders> Orders => Set<Orders>();
        public DbSet<OrderItems> OrderItems => Set<OrderItems>();
        public DbSet<OrderItemModifier> OrderItemModifier => Set<OrderItemModifier>();
        public DbSet<Shifts> Shifts => Set<Shifts>();
        public DbSet<Payments> Payments => Set<Payments>();
        public DbSet<Payment_Methods> Payment_Methods => Set<Payment_Methods>();
        public DbSet<CashInVoucher> CashInVouchers => Set<CashInVoucher>();
        public DbSet<CashOutVoucher> CashOutVouchers => Set<CashOutVoucher>();
        public DbSet<OrderRefund> OrderRefunds => Set<OrderRefund>();
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
                .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Fodo_DB;Integrated Security=True;MultipleActiveResultSets=True;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            return new IdentityDbContext(options);
        }
    }

}
