using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class UserDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public bool IsAdministrator { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
