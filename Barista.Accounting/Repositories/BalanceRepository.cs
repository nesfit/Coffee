using System;
using System.Threading.Tasks;
using Barista.Accounting.Domain;
using Barista.Accounting.Queries;

namespace Barista.Accounting.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly ISpendingRepository _spendingRepository;

        public BalanceRepository(ISpendingRepository spendingRepository)
        {
            _spendingRepository = spendingRepository;
        }

        public async Task<Balance> GetAsync(Guid ofUserId)
        {
            return new Balance(
                ofUserId,
                await _spendingRepository.GetSpendingOfUser(new GetSpendingOfUser {UserId = ofUserId})
            );
        }
    }
}
