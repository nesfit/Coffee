using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Accounting.Domain;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Barista.Accounting.Repositories
{
    public class SalesRepository : CrudRepository<AccountingDbContext, Sale>, ISalesRepository
    {
        public SalesRepository(AccountingDbContext dbContext) : base(dbContext, dbc => dbc.Sales)
        {
        }

        protected override IQueryable<Sale> QueryableAccessorFunc() =>
            base.QueryableAccessorFunc().Include(s => s.StateChanges);
    }
}
