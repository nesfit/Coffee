using Barista.Common.EfCore;
using Barista.Users.Domain;

namespace Barista.Users.Repositories
{
    public class UserRepository : CrudRepository<UsersDbContext, User>, IUserRepository
    {
        public UserRepository(UsersDbContext dbContext) : base(dbContext, dbc => dbc.Users)
        {
        }
    }
}
