using System;
using System.Collections.Generic;
using System.Linq;
using Barista.Common;

namespace Barista.Accounting.Domain
{
    public class Sale : Entity
    {
        public decimal Cost { get; private set; }
        public decimal Quantity { get; private set; }
        public Guid AccountingGroupId { get; private set; }
        public Guid UserId { get; private set; }
        public Guid AuthenticationMeansId { get; private set; }
        public Guid PointOfSaleId { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid OfferId { get; private set; }
        public ICollection<SaleStateChange> StateChanges { get; protected set; } = new List<SaleStateChange>();
        public SaleState State => MostRecentStateChange?.State ?? SaleState.FundsReserved;
        public SaleStateChange MostRecentStateChange => StateChanges.OrderBy(st => st.Created).LastOrDefault();

        public Sale(Guid id, decimal cost, decimal quantity, Guid accountingGroupId, Guid userId, Guid authenticationMeansId, Guid pointOfSaleId, Guid productId, Guid offerId) : base(id)
        {
            SetCost(cost);
            SetQuantity(quantity);
            SetAccountingGroupId(accountingGroupId);
            SetUserId(userId);
            SetAuthenticationMeansId(authenticationMeansId);
            SetPointOfSaleId(pointOfSaleId);
            SetProductId(productId);
            SetOfferId(offerId);
        }

        public Sale(Guid id, decimal cost, decimal quantity, Guid accountingGroupId, Guid userId, Guid authenticationMeansId, Guid pointOfSaleId, Guid productId, Guid offerId, DateTimeOffset createdAtDateTime) : base(id, createdAtDateTime, DateTimeOffset.UtcNow)
        {
            SetCost(cost);
            SetQuantity(quantity);
            SetAccountingGroupId(accountingGroupId);
            SetUserId(userId);
            SetAuthenticationMeansId(authenticationMeansId);
            SetPointOfSaleId(pointOfSaleId);
            SetProductId(productId);
            SetOfferId(offerId);
        }

        public void SetCost(decimal cost)
        {
            if (cost < decimal.Zero)
                throw new BaristaException("invalid_cost", "Cost cannot be negative");

            Cost = cost;
            SetUpdatedNow();
        }

        public void SetQuantity(decimal quantity)
        {
            if (quantity <= decimal.Zero)
                throw new BaristaException("invalid_quantity", "Quantity must be a positive number");

            Quantity = quantity;
            SetUpdatedNow();
        }

        public void SetAccountingGroupId(Guid accountingGroupId)
        {
            AccountingGroupId = accountingGroupId;
            SetUpdatedNow();
        }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
            SetUpdatedNow();
        }

        public void SetPointOfSaleId(Guid pointOfSaleId)
        {
            PointOfSaleId = pointOfSaleId;
            SetUpdatedNow();
        }

        public void SetProductId(Guid productId)
        {
            ProductId = productId;
            SetUpdatedNow();
        }

        public void SetOfferId(Guid offerId)
        {
            OfferId = offerId;
            SetUpdatedNow();
        }

        public void SetAuthenticationMeansId(Guid authenticationMeansId)
        {
            AuthenticationMeansId = authenticationMeansId;
            SetUpdatedNow();
        }

        public void AddStateChange(SaleStateChange stateChange)
        {
            AddStateChange(stateChange, MostRecentStateChange is null);
        }

        private void ValidateStateChange(SaleState destinationState, bool isInitialStateChange)
        {
            if (isInitialStateChange)
            {
                if (destinationState == SaleState.FundsReserved)
                    return;
                else
                    throw new BaristaException("invalid_sale_state_transition", $"Only the {nameof(SaleState.FundsReserved)} state can be accepted as initial");
            }

            switch (destinationState)
            {
                case SaleState.FundsReserved:
                    throw new BaristaException("invalid_sale_state_transition", $"Sale cannot transition into {nameof(SaleState.FundsReserved)}, this state is exclusively initial");

                case SaleState.Cancelled:
                    break;

                case SaleState.Confirmed:
                    if (State == SaleState.FundsReserved)
                        break;
                    else
                        throw new BaristaException("invalid_sale_state_transition", $"Sale can only transition into {nameof(SaleState.Confirmed)} from the {nameof(SaleState.FundsReserved)} state");

                default:
                    throw new NotSupportedException($"State transition restrictions not supported for state {destinationState}");
            }

            if (State == destinationState)
                throw new BaristaException("invalid_sale_state_transition", $"The sale is already in state {destinationState}");
        }

        private void AddStateChange(SaleStateChange stateChange, bool isInitialStateChange)
        {
            if (stateChange is null)
                throw new BaristaException("invalid_sale_state_change", "The state change is empty.");

            ValidateStateChange(stateChange.State, isInitialStateChange);
            StateChanges.Add(stateChange);
            SetUpdatedNow();
        }
    }
}
