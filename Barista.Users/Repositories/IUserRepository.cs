using System;
using System.Threading.Tasks;
using Barista.Common.EfCore;
using Barista.Users.Domain;

namespace Barista.Users.Repositories
{
    public interface IUserRepository : IBrowsableRepository<User>
    {
        Task<User> GetAsync(Guid id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task SaveChanges();
    }
}
