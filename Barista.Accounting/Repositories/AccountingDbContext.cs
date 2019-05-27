using Barista.Accounting.Domain;
using Barista.Accounting.Dto;
using Microsoft.EntityFrameworkCore;

namespace Barista.Accounting.Repositories
{
    public class AccountingDbContext : DbContext
    {
        public AccountingDbContext() : base()
        {
        }

        public AccountingDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbQuery<SpendingOfUsers> SpendingOfUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SaleStateChange>().HasOne<Sale>().WithMany(a => a.StateChanges).IsRequired();
            modelBuilder.Entity<Payment>().HasIndex(p => p.UserId);
            modelBuilder.Entity<Sale>().HasIndex(s => s.UserId);
            modelBuilder.Entity<Sale>().HasIndex(s => s.AccountingGroupId);
            modelBuilder.Entity<Sale>().HasIndex(s => s.PointOfSaleId);
            modelBuilder.Entity<Sale>().HasIndex(s => s.State);
            modelBuilder.Entity<Sale>().Ignore(s => s.State);
            modelBuilder.Entity<Sale>().Ignore(s => s.MostRecentStateChange);
            modelBuilder.Query<SpendingOfUsers>().ToView("SpendingOfUsers");
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
