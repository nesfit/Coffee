using Barista.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace Barista.Identity.Repositories
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext()
        {
        }

        public IdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AuthenticationMeans> MeansOfAuthentication { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssignmentToPointOfSale>();
            modelBuilder.Entity<AssignmentToUser>();
            modelBuilder.Entity<SpendingLimit>().HasOne<AssignmentToUser>().WithMany(a => a.SpendingLimits).IsRequired();
            modelBuilder.Entity<AuthenticationMeans>().HasIndex(m => m.Method);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
