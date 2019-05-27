using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class AssignmentToPointOfSaleDto
    {
        [Required]
        public Guid MeansId { get; set; }

        [Required]
        public Guid PointOfSaleId { get; set; }

        public DateTimeOffset ValidSince { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? ValidUntil { get; set; }
    }
}
