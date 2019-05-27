using System;
using Barista.Contracts.Commands.Sale;

namespace Barista.Consistency.Commands
{
    public class CancelTimedOutSale : ICancelTimedOutSale
    {
        public CancelTimedOutSale(Guid saleId)
        {
            SaleId = saleId;
        }

        public Guid SaleId { get; }
    }
}
