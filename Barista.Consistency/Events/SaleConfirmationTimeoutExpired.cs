using System;
using Barista.Contracts.Events.Sale;

namespace Barista.Consistency.Events
{
    public class SaleConfirmationTimeoutExpired : ISaleConfirmationTimeoutExpired
    {
        public Guid SaleId { get; set; }
    }
}
