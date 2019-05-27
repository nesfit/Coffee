using System;
using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowseStockItems : PagedQuery
    {
        public string DisplayName { get; set; }
        public Guid? AtPointOfSaleId { get; set; }
    }
}
