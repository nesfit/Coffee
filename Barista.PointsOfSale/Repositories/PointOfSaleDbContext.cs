using System;
using Barista.PointsOfSale.Domain;
using Microsoft.EntityFrameworkCore;

namespace Barista.PointsOfSale.Repositories
{
    public class PointsOfSaleDbContext : DbContext
    {
        public PointsOfSaleDbContext()
        {
        }

        public PointsOfSaleDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PointOfSale> PointsOfSale { get; set; }
        public DbSet<UserAuthorization> UserAuthorizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAuthorization>().HasKey(ua => new { ua.PointOfSaleId, ua.UserId });
            modelBuilder.Entity<PointOfSaleKeyValue>().HasKey(posKv => new {posKv.Id, posKv.Key});
            modelBuilder.Entity<PointOfSale>().Property(pos => pos.Features).HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
