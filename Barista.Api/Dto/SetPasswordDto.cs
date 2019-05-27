using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class SetPasswordDto
    {
        [Required]
        public string NewPassword { get; set; }
    }
}
