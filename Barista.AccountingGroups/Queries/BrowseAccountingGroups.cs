using System.Linq;
using Barista.AccountingGroups.Domain;
using Barista.AccountingGroups.Dto;
using Barista.Common;

namespace Barista.AccountingGroups.Queries
{
    public class BrowseAccountingGroups : PagedQuery<AccountingGroupDto>, IPagedQueryImpl<AccountingGroup>
    {
        public string DisplayName { get; set; }
        
        public IQueryable<AccountingGroup> Apply(IQueryable<AccountingGroup> q)
        {
            q = q.ApplySort(SortBy);

            if (DisplayName != null)
                q = q.Where(ag => ag.DisplayName.Contains(DisplayName));

            return q;
        }
    }
}
