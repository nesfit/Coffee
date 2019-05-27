using System.Linq;
using Barista.Common;
using Barista.Identity.Domain;
using Barista.Identity.Dto;

namespace Barista.Identity.Queries
{
    public class BrowseAuthenticationMeans : PagedQuery<AuthenticationMeansDto>, IPagedQueryImpl<AuthenticationMeans>
    {
        public string Method { get; set; }
        public string Value { get; set; }

        public IQueryable<AuthenticationMeans> Apply(IQueryable<AuthenticationMeans> q)
        {
            q = q.ApplySort(SortBy);

            if (Method != null)
                q = q.Where(m => m.Method == Method);

            if (Value != null)
                q = q.Where(m => m.Value == Value);

            return q;
        }
    }
}
