using System;

namespace Barista.Contracts.Events.Sale
{
    public interface ISaleConfirmationTimeoutExpired : IEvent
    {
        Guid SaleId { get; }
    }
}
