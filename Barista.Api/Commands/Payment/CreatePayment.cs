using System;
using Barista.Contracts.Commands.Payment;

namespace Barista.Api.Commands.Payment
{
    public class CreatePayment : ICreatePayment
    {
        public CreatePayment(Guid id, decimal amount, Guid userId, string source, string externalId)
        {
            Id = id;
            Amount = amount;
            UserId = userId;
            Source = source;
            ExternalId = externalId;
        }

        public Guid Id { get; }
        public decimal Amount { get; }
        public Guid UserId { get; }
        public string Source { get; }
        public string ExternalId { get; }
    }
}
