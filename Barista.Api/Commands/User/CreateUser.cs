using System;
using Barista.Contracts.Commands.User;

namespace Barista.Api.Commands.User
{
    public class CreateUser : ICreateUser
    {
        public CreateUser(Guid id, string fullName, string emailAddress, bool isAdministrator, bool isActive)
        {
            Id = id;
            FullName = fullName;
            EmailAddress = emailAddress;
            IsAdministrator = isAdministrator;
            IsActive = isActive;
        }

        public Guid Id { get; }
        public string FullName { get; }
        public string EmailAddress { get; }
        public bool IsAdministrator { get; }
        public bool IsActive { get; }
    }
}
