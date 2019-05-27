using System;

namespace Barista.Users.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsActive { get; set; }
    }
}
