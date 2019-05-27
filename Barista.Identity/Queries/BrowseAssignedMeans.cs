using System;
using System.Linq;
using Barista.Common;
using Barista.Identity.Dto;

namespace Barista.Identity.Queries
{
    public class BrowseAssignedMeans : PagedQuery<AuthenticationMeansWithValueDto>, IPagedQueryImpl<Domain.AuthenticationMeans>
    {
        public string Method { get; set; }
        public bool IsValid { get; set; }

        public virtual IQueryable<Domain.AuthenticationMeans> Apply(IQueryable<Domain.AuthenticationMeans> q)
        {
            var now = DateTimeOffset.Now;
            var query = q.OrderBy(means => means.Id).AsQueryable();

            if (IsValid)
                query = query.Where(means => means.ValidSince <= now).Where(means => means.ValidUntil == null || means.ValidUntil > now);

            if (Method is string requiredMethod)
                query = query.Where(means => means.Method == requiredMethod);

            return query;
        }
    }
}
