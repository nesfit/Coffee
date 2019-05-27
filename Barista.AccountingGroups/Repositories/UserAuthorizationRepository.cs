using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Domain;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Barista.AccountingGroups.Repositories
{
    public class UserAuthorizationRepository : IUserAuthorizationRepository
    {
        private readonly AccountingGroupsDbContext _dbContext;

        public UserAuthorizationRepository(AccountingGroupsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IPagedResult<UserAuthorization>> BrowseAsync(IPagedQueryImpl<UserAuthorization> query)
        {
            return await _dbContext.UserAuthorizations.PaginateAsync(query);
        }

        public async Task<UserAuthorization> GetAsync(Guid accountingGroupId, Guid userId)
        {
            return await _dbContext.UserAuthorizations.FindAsync(accountingGroupId, userId);
        }

        public async Task AddAsync(UserAuthorization userAuthorization)
        {
            await _dbContext.UserAuthorizations.AddAsync(userAuthorization);
        }

        public Task UpdateAsync(UserAuthorization userAuthorization)
        {
            _dbContext.UserAuthorizations.Update(userAuthorization);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(UserAuthorization userAuthorization)
        {
            _dbContext.UserAuthorizations.Remove(userAuthorization);
            return Task.CompletedTask;
        }

        public async Task SaveChanges()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e) when (e.InnerException.Message.StartsWith("Duplicate entry"))
            {
                throw new EntityAlreadyExistsException(e);
            }
        }
    }
}
