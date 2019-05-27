using System;
using System.Collections.Generic;
using Barista.Api.ResourceAuthorization;

namespace Barista.Api.Models.PointsOfSale
{
    public class PointOfSaleAuthorizedUser : IUserAuthorizationLevel
    {
        private static readonly IList<string> PointOfSaleAuthorizedUserLevels = new[] {"AuthorizedUser", "Owner"};

        public Guid PointOfSaleId { get; set; }
        public Guid UserId { get; set; }
        public string Level { get; set; }
        
        public int CompareTo(IUserAuthorizationLevel userAuthorizationLevel)
        {
            if (userAuthorizationLevel == null)
                return 1;

            var otherIndex = PointOfSaleAuthorizedUserLevels.IndexOf(userAuthorizationLevel.Level);
            var ownIndex = PointOfSaleAuthorizedUserLevels.IndexOf(Level);

            if (ownIndex == -1)
                throw new InvalidOperationException("Instance has unrecognized point of sale user authorization level value");

            return ownIndex.CompareTo(otherIndex);
        }
    }
}
