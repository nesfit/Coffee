using System;
using System.ComponentModel.DataAnnotations;
using Barista.Contracts.Commands.Swipe;

namespace Barista.Api.Commands.Swipe
{
    public class ConfirmSwipe : IConfirmSwipe
    {
        [Required]
        public Guid PointOfSaleId { get; set; }

        [Required]
        public Guid SaleId { get; set; }
    }
}
