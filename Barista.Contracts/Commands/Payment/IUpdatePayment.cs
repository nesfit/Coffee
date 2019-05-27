using System;

namespace Barista.Contracts.Commands.Payment
{
    public interface IUpdatePayment : ICommand
    {
        Guid Id { get; }
        decimal Amount { get; }
        Guid UserId { get; }
        string Source { get; }
        string ExternalId { get; }
    }
}
