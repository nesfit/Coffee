using System.Linq;
using Barista.Common;
using Barista.Products.Domain;
using Barista.Products.Dto;

namespace Barista.Products.Queries
{
    public class BrowseProducts : PagedQuery<ProductDto>, IPagedQueryImpl<Product>
    {
        public string DisplayName { get; set; }

        public IQueryable<Product> Apply(IQueryable<Product> q)
        {
            q = q.ApplySort(SortBy);

            if (DisplayName != null)
                q = q.Where(product => product.DisplayName.Contains(DisplayName));

            return q;
        }
    }
}
