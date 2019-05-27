using System;
using Barista.Contracts.Commands.User;

namespace Barista.Consistency.Commands
{
    public class DeleteUser : IDeleteUser
    {
        public Guid Id { get; set; }
    }
}
