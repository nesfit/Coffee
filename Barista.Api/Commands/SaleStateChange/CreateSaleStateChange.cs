using System;
using Barista.Contracts.Commands.SaleStateChange;

namespace Barista.Api.Commands.SaleStateChange
{
    public class CreateSaleStateChange : ICreateSaleStateChange
    {
        public CreateSaleStateChange(Guid parentSaleId, Guid id, string reason, string state, Guid? causedByPointOfSaleId, Guid? causedByUserId)
        {
            ParentSaleId = parentSaleId;
            Id = id;
            Reason = reason;
            State = state;
            CausedByPointOfSaleId = causedByPointOfSaleId;
            CausedByUserId = causedByUserId;
        }

        public Guid ParentSaleId { get;  }
        public Guid Id { get; }
        public string Reason { get; }
        public string State { get; }
        public Guid? CausedByPointOfSaleId { get; }
        public Guid? CausedByUserId { get; }
    }
}
