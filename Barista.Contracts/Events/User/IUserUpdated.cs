using System;

namespace Barista.Contracts.Events.User
{
    public interface IUserUpdated : IEvent
    {
        Guid Id { get; }
        string FullName { get; }
        string EmailAddress { get; }
        bool IsAdministrator { get; }
        bool IsActive { get; }
    }
}
