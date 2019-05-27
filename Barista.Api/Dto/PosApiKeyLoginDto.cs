using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class PosApiKeyLoginDto
    {
        [Required]
        public Guid PointOfSaleId { get; set; }

        [Required]
        public string Key { get; set; }
    }
}
