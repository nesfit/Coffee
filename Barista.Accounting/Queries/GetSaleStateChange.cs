using System;
using Barista.Accounting.Dto;
using Barista.Common;
using Barista.Contracts;

namespace Barista.Accounting.Queries
{
    public class GetSaleStateChange : IQuery<SaleStateChangeDto>
    {
        public GetSaleStateChange(Guid saleStateChangeId, Guid parentSaleId)
        {
            SaleStateChangeId = saleStateChangeId;
            ParentSaleId = parentSaleId;
        }

        public Guid SaleStateChangeId { get; }
        public Guid ParentSaleId { get; }
    }
}
