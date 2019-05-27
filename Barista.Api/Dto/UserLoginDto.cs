using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class UserLoginDto
    {
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
