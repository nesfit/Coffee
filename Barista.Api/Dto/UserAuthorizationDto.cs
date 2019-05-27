using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class UserAuthorizationDto
    {
        [Required]
        public string Level { get; set; }
    }
}
