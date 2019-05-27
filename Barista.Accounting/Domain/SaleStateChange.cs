using System;
using Barista.Common;

namespace Barista.Accounting.Domain
{
    public class SaleStateChange : Entity
    {
        public string Reason { get; private set; }
        public SaleState State { get; private set; }
        public Guid? CausedByPointOfSaleId { get; private set; }
        public Guid? CausedByUserId { get; private set; }

        public SaleStateChange(Guid id, string reason, SaleState state, Guid? causedByPointOfSaleId, Guid? causedByUserId) : base(id)
        {
            SetReason(reason);
            SetState(state);
            SetCausedByPointOfSaleId(causedByPointOfSaleId);
            SetCausedByUserId(causedByUserId);
        }

        public void SetReason(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new BaristaException("invalid_reason", "Reason cannot be empty.");

            Reason = reason;
            SetUpdatedNow();
        }

        public void SetState(SaleState state)
        {
            State = state;
            SetUpdatedNow();
        }

        public void SetCausedByPointOfSaleId(Guid? pointOfSaleId)
        {
            CausedByPointOfSaleId = pointOfSaleId;
            SetUpdatedNow();
        }

        public void SetCausedByUserId(Guid? userId)
        {
            CausedByUserId = userId;
            SetUpdatedNow();
        }

    }
}
