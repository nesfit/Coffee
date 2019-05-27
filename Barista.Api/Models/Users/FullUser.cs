using System;

namespace Barista.Api.Models.Users
{
    public class FullUser : User
    {
        public string EmailAddress { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsActive { get; set; }
    }
}
