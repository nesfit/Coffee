using System;
using Barista.Contracts.Events.Payment;

namespace Barista.Accounting.Events.Payment
{
    public class PaymentUpdated : IPaymentUpdated
    {
        public Guid Id { get; }
        public decimal Amount { get; }
        public Guid UserId { get; }
        public string Source { get; }
        public string ExternalId { get; }

        public PaymentUpdated(Guid id, decimal amount, Guid userId, string source, string externalId)
        {
            Id = id;
            Amount = amount;
            UserId = userId;
            Source = source;
            ExternalId = externalId;
        }

        public PaymentUpdated(Domain.Payment payment) : this(payment.Id, payment.Amount, payment.UserId, payment.Source, payment.ExternalId)
        {
            
        }
    }
}
