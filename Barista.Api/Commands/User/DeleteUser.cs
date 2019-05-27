using System;
using Barista.Contracts.Commands.User;

namespace Barista.Api.Commands.User
{
    public class DeleteUser : IDeleteUser
    {
        public DeleteUser(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
