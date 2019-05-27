using System;
using System.Threading.Tasks;
using Barista.Common.EfCore;
using Barista.PointsOfSale.Domain;

namespace Barista.PointsOfSale.Repositories
{
    public interface IPointOfSaleRepository : IBrowsableRepository<PointOfSale>
    {
        Task<PointOfSale> GetAsync(Guid id);
        Task AddAsync(PointOfSale pointOfSale);
        Task UpdateAsync(PointOfSale pointOfSale);
        Task DeleteAsync(PointOfSale pointOfSale);
        Task SaveChanges();
    }
}
