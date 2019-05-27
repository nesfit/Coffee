using System;
using System.Linq.Expressions;
using Barista.Api.Models.AccountingGroups;
using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowseAccountingGroupAuthorizedUsers : PagedQuery
    {
        public Guid? UserId { get; set; }
        public Guid? AccountingGroupId { get; set; }
        public string UserAuthorizationLevel { get; set; }
    }
}
