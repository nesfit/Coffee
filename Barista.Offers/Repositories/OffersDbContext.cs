using Barista.Offers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Barista.Offers.Repositories
{
    public class OffersDbContext : DbContext
    {
        public OffersDbContext()
        {

        }

        public OffersDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Offer> Offers { get; set; }
    }
}
