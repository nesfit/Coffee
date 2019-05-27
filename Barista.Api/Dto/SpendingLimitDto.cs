using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class SpendingLimitDto
    {
        [Required]
        public TimeSpan Interval { get; set; }

        [Required]
        public decimal Value { get; set; }
    }
}
