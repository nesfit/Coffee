using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Domain;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Contracts;

namespace Barista.AccountingGroups.Repositories
{
    public interface IUserAuthorizationRepository : IBrowsableRepository<UserAuthorization>
    {
        Task<UserAuthorization> GetAsync(Guid accountingGroupId, Guid userId);
        Task AddAsync(UserAuthorization userAuthorization);
        Task UpdateAsync(UserAuthorization userAuthorization);
        Task DeleteAsync(UserAuthorization userAuthorization);
        Task SaveChanges();
    }
}
