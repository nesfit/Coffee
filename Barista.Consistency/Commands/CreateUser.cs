using System;
using Barista.Contracts.Commands.User;

namespace Barista.Consistency.Commands
{
    public class CreateUser : ICreateUser
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsActive { get; set; }
    }
}
