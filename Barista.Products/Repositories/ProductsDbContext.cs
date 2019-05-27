using Barista.Products.Domain;
using Microsoft.EntityFrameworkCore;

namespace Barista.Products.Repositories
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext()
        {
        }

        public ProductsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
