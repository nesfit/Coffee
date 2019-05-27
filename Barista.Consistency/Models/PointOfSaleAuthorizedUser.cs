using System;

namespace Barista.Consistency.Models
{
    public class PointOfSaleAuthorizedUser
    {
        public Guid PointOfSaleId { get; set; }
        public Guid UserId { get; set; }
    }
}
