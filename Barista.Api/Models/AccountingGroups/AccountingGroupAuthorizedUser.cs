using System;
using System.Collections.Generic;
using Barista.Api.ResourceAuthorization;
using MassTransit;

namespace Barista.Api.Models.AccountingGroups
{
    public class AccountingGroupAuthorizedUser : IUserAuthorizationLevel
    {
        private static readonly IList<string> AccountingGroupAuthorizedUserLevels = new[] {"AuthorizedUser", "Owner"};

        public Guid AccountingGroupId { get; set; }
        public Guid UserId { get; set; }
        public string Level { get; set; }

        public int CompareTo(IUserAuthorizationLevel userAuthorizationLevel)
        {
            if (userAuthorizationLevel == null)
                return 1;

            var otherIndex = AccountingGroupAuthorizedUserLevels.IndexOf(userAuthorizationLevel.Level);
            var ownIndex = AccountingGroupAuthorizedUserLevels.IndexOf(Level);

            if (ownIndex == -1)
                throw new InvalidOperationException("Instance has unrecognized accounting group user authorization level value");

            return ownIndex.CompareTo(otherIndex);
        }
    }
}
