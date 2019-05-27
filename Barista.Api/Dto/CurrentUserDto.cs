using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class CurrentUserDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
