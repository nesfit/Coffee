using System;
using System.Threading.Tasks;
using Barista.Common.EfCore;
using Barista.SaleStrategies.Domain;

namespace Barista.SaleStrategies.Repositories
{
    public interface ISaleStrategyRepository : IBrowsableRepository<SaleStrategy>
    {
        Task<SaleStrategy> GetSaleStrategy(Guid id);
    }
}
