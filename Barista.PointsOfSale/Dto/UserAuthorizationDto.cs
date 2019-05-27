using System;

namespace Barista.PointsOfSale.Dto
{
    public class UserAuthorizationDto
    {
        public Guid PointOfSaleId { get; set; }
        public Guid UserId { get; set; }
        public string Level { get; set; }
    }
}
