using System;

namespace Barista.Api.Models.Identity
{
    public class AssignmentToPointOfSale : Assignment
    {
        public Guid AssignedToPointOfSaleId { get; set; }
    }
}
