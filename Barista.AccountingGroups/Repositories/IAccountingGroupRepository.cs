using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Domain;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Contracts;

namespace Barista.AccountingGroups.Repositories
{
    public interface IAccountingGroupRepository : IBrowsableRepository<AccountingGroup>
    {
        Task<AccountingGroup> GetAsync(Guid id);
        Task AddAsync(AccountingGroup accountingGroup);
        Task UpdateAsync(AccountingGroup accountingGroup);
        Task DeleteAsync(AccountingGroup accountingGroup);
        Task SaveChanges();
    }
}
