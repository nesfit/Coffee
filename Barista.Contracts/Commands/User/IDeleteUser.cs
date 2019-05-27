using System;

namespace Barista.Contracts.Commands.User
{
    public interface IDeleteUser : ICommand
    {
        Guid Id { get; }
    }
}
