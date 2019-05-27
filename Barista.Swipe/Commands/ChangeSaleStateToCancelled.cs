using System;
using Barista.Contracts.Commands.SaleStateChange;

namespace Barista.Swipe.Commands
{
    public class ChangeSaleStateToCancelled : ICreateSaleStateChange
    {
        public ChangeSaleStateToCancelled(Guid parentSaleId, Guid id, string reason, Guid causedByPointOfSaleId)
        {
            ParentSaleId = parentSaleId;
            Id = id;
            Reason = reason;
            CausedByPointOfSaleId = causedByPointOfSaleId;
        }

        public Guid ParentSaleId { get; }
        public Guid Id { get; }
        public string Reason { get; }
        public string State => "Cancelled";
        public Guid? CausedByPointOfSaleId { get; }
        public Guid? CausedByUserId => null;
    }
}
