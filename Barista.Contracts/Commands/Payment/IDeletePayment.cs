using System;

namespace Barista.Contracts.Commands.Payment
{
    public interface IDeletePayment : ICommand
    {
        Guid Id { get; }
    }
}
