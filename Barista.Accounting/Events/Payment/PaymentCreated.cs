using System;
using Barista.Contracts.Events.Payment;

namespace Barista.Accounting.Events.Payment
{
    public class PaymentCreated : IPaymentCreated
    {
        public Guid Id { get; }
        public decimal Amount { get; }
        public Guid UserId { get; }
        public string Source { get; }
        public string ExternalId { get; }

        public PaymentCreated(Guid id, decimal amount, Guid userId, string source, string externalId)
        {
            Id = id;
            Amount = amount;
            UserId = userId;
            Source = source;
            ExternalId = externalId;
        }

        public PaymentCreated(Domain.Payment payment) : this(payment.Id, payment.Amount, payment.UserId, payment.Source, payment.ExternalId)
        {
            
        }


    }
}
