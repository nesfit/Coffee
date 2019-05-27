using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Contracts;
using Barista.Identity.Domain;

namespace Barista.Identity.Repositories
{
    public class AssignmentsRepository : CrudRepository<IdentityDbContext, Assignment>, IAssignmentsRepository
    {
        public AssignmentsRepository(IdentityDbContext dbContext) : base(dbContext, c => c.Assignments)
        {
        }

        public async Task<IPagedResult<AssignmentToUser>> BrowseAsync(IPagedQueryImpl<AssignmentToUser> pagedQuery)
        {
            if (pagedQuery == null) throw new ArgumentNullException(nameof(pagedQuery));
            return await DbContext.Assignments.OfType<AssignmentToUser>().PaginateAsync(pagedQuery);
        }

        public async Task<IPagedResult<AssignmentToPointOfSale>> BrowseAsync(IPagedQueryImpl<AssignmentToPointOfSale> pagedQuery)
        {
            if (pagedQuery == null) throw new ArgumentNullException(nameof(pagedQuery));
            return await DbContext.Assignments.OfType<AssignmentToPointOfSale>().PaginateAsync(pagedQuery);
        }
    }
}
