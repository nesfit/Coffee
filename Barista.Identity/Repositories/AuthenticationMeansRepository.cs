using Barista.Common.EfCore;
using Barista.Identity.Domain;

namespace Barista.Identity.Repositories
{
    public class AuthenticationMeansRepository : CrudRepository<IdentityDbContext, AuthenticationMeans>, IAuthenticationMeansRepository
    {
        public AuthenticationMeansRepository(IdentityDbContext dbContext) : base(dbContext, c => c.MeansOfAuthentication)
        {
        }
    }
}
