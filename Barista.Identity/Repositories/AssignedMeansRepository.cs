using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Contracts;
using Barista.Identity.Domain;

namespace Barista.Identity.Repositories
{
    public class AssignedMeansRepository : IAssignedMeansRepository
    {
        private readonly IdentityDbContext _dbContext;

        public AssignedMeansRepository(IdentityDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IPagedResult<AuthenticationMeans>> BrowseMeansAssignedToUser(Guid userId, IPagedQueryImpl<AuthenticationMeans> query)
        {
            var now = DateTimeOffset.UtcNow;

            return await _dbContext.Assignments.OfType<AssignmentToUser>().Where(ass => ass.UserId == userId)
                .Join(_dbContext.MeansOfAuthentication, ass => ass.MeansId, means => means.Id, (ass, means) => means)
                .PaginateAsync(query);
        }

        public async Task<IPagedResult<AuthenticationMeans>> BrowseMeansAssignedToPointOfSale(Guid posId, IPagedQueryImpl<AuthenticationMeans> query)
        {
            var now = DateTimeOffset.UtcNow;

            return await _dbContext.Assignments.OfType<AssignmentToPointOfSale>().Where(ass => ass.PointOfSaleId == posId)
                .Join(_dbContext.MeansOfAuthentication, ass => ass.MeansId, means => means.Id, (ass, means) => means)
                .PaginateAsync(query);
        }
    }
}
