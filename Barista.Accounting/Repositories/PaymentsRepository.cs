using Barista.Accounting.Domain;
using Barista.Common.EfCore;

namespace Barista.Accounting.Repositories
{
    public class PaymentsRepository : CrudRepository<AccountingDbContext, Payment>, IPaymentsRepository
    {
        public PaymentsRepository(AccountingDbContext dbContext) : base(dbContext, dbc => dbc.Payments)
        {
        }
    }
}
