using System;
using Barista.Common;

namespace Barista.Users.Domain
{
    public class User : Entity
    {
        public string FullName { get; private set; }
        public string EmailAddress { get; private set; }
        public bool IsAdministrator { get; private set; }
        public bool IsActive { get; private set; }

        public User(Guid id, string fullName, string emailAddress, bool isAdministrator, bool isActive) : base(id)
        {
            SetFullName(fullName);
            SetEmailAddress(emailAddress);
            SetIsAdministrator(isAdministrator);
            SetIsActive(isActive);
        }

        public void SetFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new BaristaException("invalid_full_name", "Full name cannot be empty");

            FullName = fullName;
            SetUpdatedNow();
        }

        public void SetEmailAddress(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
                throw new BaristaException("invalid_email_address", "Email address cannot be empty");

            EmailAddress = emailAddress;
            SetUpdatedNow();
        }

        public void SetIsAdministrator(bool isAdministrator)
        {
            IsAdministrator = isAdministrator;
            SetUpdatedNow();
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
            SetUpdatedNow();
        }
    }
}
