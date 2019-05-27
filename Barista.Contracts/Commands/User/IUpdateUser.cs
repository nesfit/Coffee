using System;

namespace Barista.Contracts.Commands.User
{
    public interface IUpdateUser : ICommand
    {
        Guid Id { get; }
        string FullName { get; }
        string EmailAddress { get; }
        bool IsAdministrator { get; }
        bool IsActive { get; }
    }
}
