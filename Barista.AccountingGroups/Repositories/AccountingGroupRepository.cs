using Barista.AccountingGroups.Domain;
using Barista.Common.EfCore;

namespace Barista.AccountingGroups.Repositories
{
    public class AccountingGroupRepository : CrudRepository<AccountingGroupsDbContext, AccountingGroup>, IAccountingGroupRepository
    {
        public AccountingGroupRepository(AccountingGroupsDbContext dbContext) : base(dbContext, dbc => dbc.AccountingGroups)
        {
        }
    }
}
