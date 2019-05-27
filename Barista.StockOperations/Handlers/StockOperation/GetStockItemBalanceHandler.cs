using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.StockOperations.Queries;
using Barista.StockOperations.Repositories;

namespace Barista.StockOperations.Handlers.StockOperation
{
    public class GetStockItemBalanceHandler : IQueryHandler<GetStockItemBalance, decimal>
    {
        private readonly IStockOperationRepository _repository;

        public GetStockItemBalanceHandler(IStockOperationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<decimal> HandleAsync(GetStockItemBalance query)
        {
            return await _repository.GetStockItemBalance(query.StockItemId);
        }
    }
}
