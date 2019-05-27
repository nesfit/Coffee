using Barista.Common.EfCore;
using Barista.Products.Domain;

namespace Barista.Products.Repositories
{
    public class ProductRepository : CrudRepository<ProductsDbContext, Product>, IProductRepository
    {
        public ProductRepository(ProductsDbContext dbContext) : base(dbContext, dbc => dbc.Products)
        {
        }
    }
}
