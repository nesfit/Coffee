using System;
using System.Linq;
using Barista.Common;
using Barista.PointsOfSale.Domain;
using Barista.PointsOfSale.Dto;

namespace Barista.PointsOfSale.Queries
{
    public class BrowseUserAuthorizations : PagedQuery<UserAuthorizationDto>, IPagedQueryImpl<UserAuthorization>
    {
        public Guid? UserId { get; set; }
        public Guid? PointOfSaleId { get; set; }
        public string UserAuthorizationLevel { get; set; }

        public IQueryable<UserAuthorization> Apply(IQueryable<UserAuthorization> q)
        {
            q = q.OrderBy(ua => ua.UserId);

            if (UserId is Guid userId)
                q = q.Where(ua => ua.UserId == userId);

            if (PointOfSaleId is Guid posId)
                q = q.Where(ua => ua.PointOfSaleId == posId);

            if (!string.IsNullOrWhiteSpace(UserAuthorizationLevel))
            {
                if (Enum.TryParse<UserAuthorizationLevel>(UserAuthorizationLevel, out var level))
                    q = q.Where(ua => ua.Level == level);
                else
                    throw new BaristaException("invalid_user_authorization_level", $"Unknown user authorization level value '{level}'");
            }

            return q;
        }
    }
}
