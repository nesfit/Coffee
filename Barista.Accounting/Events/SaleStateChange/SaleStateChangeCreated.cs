using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Contracts.Events.SaleStateChange;

namespace Barista.Accounting.Events.SaleStateChange
{
    public class SaleStateChangeCreated : ISaleStateChangeCreated
    {
        public Guid Id { get; }
        public Guid SaleId { get; }
        public DateTimeOffset Created { get; }
        public string Reason { get; }
        public string State { get; }
        public Guid? CausedByPointOfSaleId { get; }
        public Guid? CausedByUserId { get; }

        public SaleStateChangeCreated(Guid id, Guid saleId, DateTimeOffset created, string reason, string state, Guid? causedByPointOfSaleId, Guid? causedByUserId)
        {
            Id = id;
            SaleId = saleId;
            Created = created;
            Reason = reason;
            State = state;
            CausedByPointOfSaleId = causedByPointOfSaleId;
            CausedByUserId = causedByUserId;
        }
    }
}
