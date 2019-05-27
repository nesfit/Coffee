using System;
using Barista.Contracts.Events.User;

namespace Barista.Users.Events.User
{
    public class UserDeleted : IUserDeleted
    {
        public UserDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
