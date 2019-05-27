using System;

namespace Barista.Api.Queries
{
    public class BrowsePointsOfSale : DisplayNameQuery
    {
        public Guid? ParentAccountingGroupId { get; set; }
    }
}
