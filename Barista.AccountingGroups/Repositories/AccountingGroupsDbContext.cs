using Barista.AccountingGroups.Domain;
using Microsoft.EntityFrameworkCore;

namespace Barista.AccountingGroups.Repositories
{
    public class AccountingGroupsDbContext : DbContext
    {
        public AccountingGroupsDbContext() : base()
        {
        }

        public AccountingGroupsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AccountingGroup> AccountingGroups { get; set; }
        public DbSet<UserAuthorization> UserAuthorizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAuthorization>().HasKey(ua => new { ua.AccountingGroupId, ua.UserId });
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
