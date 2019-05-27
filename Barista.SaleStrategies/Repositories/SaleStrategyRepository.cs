using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Contracts;
using Barista.SaleStrategies.Domain;

namespace Barista.SaleStrategies.Repositories
{
    public class SaleStrategyRepository : ISaleStrategyRepository
    { 
        private readonly IReadOnlyDictionary<Guid, SaleStrategy> _saleStrategiesDict;
        private readonly ICollection<SaleStrategy> _saleStrategies;
        private readonly IClientsidePaginator<SaleStrategy> _paginator;

        public SaleStrategyRepository(IClientsidePaginator<SaleStrategy> paginator)
        {
            _paginator = paginator ?? throw new ArgumentNullException(nameof(paginator));

            var dict = new ConcurrentDictionary<Guid, SaleStrategy>();
            var strategies = new SaleStrategy[] {new UnlimitedStrategy(), new CreditStrategy()};
            foreach (var strategy in strategies)
                dict.TryAdd(strategy.Id, strategy);

            _saleStrategiesDict = dict;
            _saleStrategies = strategies;
        }

        public Task<SaleStrategy> GetSaleStrategy(Guid id)
        {
            return Task.FromResult(_saleStrategiesDict.TryGetValue(id, out var strategy) ? strategy : null);
        }

        public async Task<IPagedResult<SaleStrategy>> BrowseAsync(IPagedQueryImpl<SaleStrategy> query)
        {
            return await _paginator.PaginateAsync(_saleStrategies, query);
        }
    }
}
