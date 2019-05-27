using System;
using System.Threading.Tasks;
using Barista.Accounting.Domain;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Contracts;

namespace Barista.Accounting.Repositories
{
    public interface ISalesRepository : IBrowsableRepository<Sale>
    {
        Task<Sale> GetAsync(Guid id);
        Task AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(Sale sale);
        Task SaveChanges();
    }
}
