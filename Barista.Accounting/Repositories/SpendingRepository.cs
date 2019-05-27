using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Accounting.Domain;
using Barista.Accounting.Queries;
using Barista.Common;
using Barista.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Barista.Accounting.Repositories
{
    public class SpendingRepository : ISpendingRepository
    {
        private readonly AccountingDbContext _dbContext;

        public SpendingRepository(AccountingDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<decimal> GetSpendingOfMeans(Guid authenticationMeansId, DateTimeOffset? since)
        {
            var dbQuery = _dbContext.Sales.Where(sale => sale.AuthenticationMeansId == authenticationMeansId);

            if (since != null)
                dbQuery = dbQuery.Where(sale => sale.Created >= since);

            return await dbQuery.SumAsync(sale => sale.Cost);
        }

        public async Task<decimal> GetSpendingOfUser(GetSpendingOfUser query)
            => await query.Apply(_dbContext.SpendingOfUsers.AsQueryable()).Select(sp => sp.Amount).SumAsync();

        public async Task<IPagedResult<SpendingOfUsers>> BrowseAsync(IPagedQueryImpl<SpendingOfUsers> query)
            => await _dbContext.SpendingOfUsers.PaginateAsync(query);
    }
}
