using System;
using Barista.Contracts.Commands.Payment;

namespace Barista.Api.Commands.Payment
{
    public class DeletePayment : IDeletePayment
    {
        public DeletePayment(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
