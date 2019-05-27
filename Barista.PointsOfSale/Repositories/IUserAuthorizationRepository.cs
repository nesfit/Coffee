using System;
using System.Threading.Tasks;
using Barista.Common.EfCore;
using Barista.PointsOfSale.Domain;

namespace Barista.PointsOfSale.Repositories
{
    public interface IUserAuthorizationRepository : IBrowsableRepository<UserAuthorization>
    {
        Task<UserAuthorization> GetAsync(Guid pointOfSaleId, Guid userId);
        Task AddAsync(UserAuthorization userAuthorization);
        Task UpdateAsync(UserAuthorization userAuthorization);
        Task DeleteAsync(UserAuthorization userAuthorization);
        Task SaveChanges();
    }
}
