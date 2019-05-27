using System;
using Barista.Contracts.Events.SaleStateChange;

namespace Barista.Accounting.Events.SaleStateChange
{
    public class SaleStateChangeUpdated : ISaleStateChangeUpdated
    {
        public SaleStateChangeUpdated(Guid id, DateTimeOffset created, string reason, string state, Guid? causedByPointOfSaleId, Guid? causedByUserId)
        {
            Id = id;
            Created = created;
            Reason = reason;
            State = state;
            CausedByPointOfSaleId = causedByPointOfSaleId;
            CausedByUserId = causedByUserId;
        }

        public Guid Id { get; }
        public DateTimeOffset Created { get; }
        public string Reason { get; }
        public string State { get; }
        public Guid? CausedByPointOfSaleId { get; }
        public Guid? CausedByUserId { get; }
    }
}
