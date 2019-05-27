using System;
using System.Threading.Tasks;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;

namespace Barista.Accounting.Handlers.Spending
{
    public class GetSpendingOfMeansHandler : IQueryHandler<GetSpendingOfMeans, decimal>
    {
        private readonly ISpendingRepository _spendingRepository;

        public GetSpendingOfMeansHandler(ISpendingRepository spendingRepository)
        {
            _spendingRepository = spendingRepository ?? throw new ArgumentNullException(nameof(spendingRepository));
        }
        
        public async Task<decimal> HandleAsync(GetSpendingOfMeans query)
        {
            return await _spendingRepository.GetSpendingOfMeans(query.AuthenticationMeansId, query.Since);
        }
    }
}
