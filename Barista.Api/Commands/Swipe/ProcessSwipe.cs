using System;
using System.ComponentModel.DataAnnotations;
using Barista.Contracts.Commands.Swipe;

namespace Barista.Api.Commands.Swipe
{
    public class ProcessSwipe : IProcessSwipe
    {
        [Required]
        public string AuthenticationMeansMethod { get; set; }

        [Required]
        public string AuthenticationMeansValue { get; set; }

        [Required]
        public Guid PointOfSaleId { get; set; }

        [Required]
        public Guid OfferId { get; set; }

        [Required]
        public decimal Quantity { get; set; }
    }
}
