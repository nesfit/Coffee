using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class UserApiKeyLoginDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Key { get; set; }
    }
}
