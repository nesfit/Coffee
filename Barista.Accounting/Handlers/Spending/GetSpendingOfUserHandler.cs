using System;
using System.Threading.Tasks;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;

namespace Barista.Accounting.Handlers.Spending
{
    public class GetSpendingOfUserHandler : IQueryHandler<GetSpendingOfUser, decimal>
    {
        private readonly ISpendingRepository _repository;

        public GetSpendingOfUserHandler(ISpendingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<decimal> HandleAsync(GetSpendingOfUser query)
        {
            return await _repository.GetSpendingOfUser(query);
        }
    }
}
