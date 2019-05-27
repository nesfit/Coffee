using System;
using System.Threading.Tasks;
using Barista.Common.EfCore;
using Barista.Identity.Domain;

namespace Barista.Identity.Repositories
{
    public interface IAuthenticationMeansRepository : IBrowsableRepository<AuthenticationMeans>
    {
        Task<AuthenticationMeans> GetAsync(Guid id);
        Task AddAsync(AuthenticationMeans authenticationMeans);
        Task UpdateAsync(AuthenticationMeans authenticationMeans);
        Task DeleteAsync(AuthenticationMeans authenticationMeans);
        Task SaveChanges();
    }
}
