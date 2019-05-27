using System;

namespace Barista.Swipe.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
    }
}
