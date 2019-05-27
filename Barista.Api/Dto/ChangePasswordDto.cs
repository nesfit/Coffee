using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string NewPasswordAgain { get; set; }
    }
}
