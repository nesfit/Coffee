using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class PaymentDto
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Source { get; set; }

        public string ExternalId { get; set; }
    }
}
