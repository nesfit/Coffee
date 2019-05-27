using System;
using System.ComponentModel.DataAnnotations;
using Barista.Contracts.Commands.Swipe;

namespace Barista.Api.Commands.Swipe
{
    public class CancelSwipe : ICancelSwipe
    {
        [Required]
        public Guid PointOfSaleId { get; set; }

        [Required]
        public Guid SaleId { get; set; }
    }
}
