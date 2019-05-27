using System;
using Barista.Contracts.Events.Payment;

namespace Barista.Accounting.Events.Payment
{
    public class PaymentDeleted : IPaymentDeleted
    {
        public Guid Id { get; }

        public PaymentDeleted(Guid id)
        {
            Id = id;
        }
    }
}
