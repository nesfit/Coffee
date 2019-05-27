using System;
using System.Linq;
using Barista.AccountingGroups.Domain;
using Barista.AccountingGroups.Dto;
using Barista.Common;

namespace Barista.AccountingGroups.Queries
{
    public class BrowseUserAuthorizations : PagedQuery<UserAuthorizationDto>, IPagedQueryImpl<UserAuthorization>
    {
        public Guid? UserId { get; set; }
        public Guid? AccountingGroupId { get; set; }
        public UserAuthorizationLevelDto? UserAuthorizationLevel { get; set; }

        public IQueryable<UserAuthorization> Apply(IQueryable<UserAuthorization> q)
        {
            q = q.OrderBy(ua => ua.UserId);

            if (UserId != null)
                q = q.Where(ua => ua.UserId == UserId);

            if (AccountingGroupId != null)
                q = q.Where(ua => ua.AccountingGroupId == AccountingGroupId);

            if (UserAuthorizationLevel != null)
            {
                UserAuthorizationLevel mappedLevel;

                switch (UserAuthorizationLevel.Value)
                {
                    case UserAuthorizationLevelDto.AuthorizedUser:
                        mappedLevel = Domain.UserAuthorizationLevel.AuthorizedUser;
                        break;

                    case UserAuthorizationLevelDto.Owner:
                        mappedLevel = Domain.UserAuthorizationLevel.Owner;
                        break;

                    default:
                        throw new BaristaException("invalid_user_authorization_level", $"Unrecognized user authorization level '{UserAuthorizationLevel}'");
                }
                
                q = q.Where(ua => ua.Level == mappedLevel);
            }

            return q;
        }
    }
}
