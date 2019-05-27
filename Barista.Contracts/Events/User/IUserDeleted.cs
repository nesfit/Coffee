using System;

namespace Barista.Contracts.Events.User
{
    public interface IUserDeleted : IEvent
    {
        Guid Id { get; }
    }
}
