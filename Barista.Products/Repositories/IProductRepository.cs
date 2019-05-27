using System;
using System.Threading.Tasks;
using Barista.Common.EfCore;
using Barista.Products.Domain;

namespace Barista.Products.Repositories
{
    public interface IProductRepository : IBrowsableRepository<Product>
    {
        Task<Product> GetAsync(Guid id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task SaveChanges();
    }
}
