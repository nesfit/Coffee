using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Consistency.Commands;
using Barista.Consistency.Services;
using Microsoft.Extensions.Logging;

namespace Barista.Consistency.Activities.User
{
    public class DeleteOrphanedAccountingGroupAuthorizationsActivity : ConsistencyRemediationActivity<UserIdParameters, Guid>
    {
        private readonly IAccountingGroupsService _agService;
        private readonly IBusPublisher _busPublisher;

        public DeleteOrphanedAccountingGroupAuthorizationsActivity(IAccountingGroupsService agService, IBusPublisher busPublisher, ILogger<DeleteOrphanedAccountingGroupAuthorizationsActivity> logger)
            : base(logger)
        {
            _agService = agService ?? throw new ArgumentNullException(nameof(agService));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        protected override async Task<IEnumerable<Guid>> FindAsync(UserIdParameters args)
            => (await _agService.BrowseUserAuthorizations(args.UserId, new PagedQuery())).Items.Select(i => i.AccountingGroupId);

        protected override async Task<IOperationResult> RemedyAsync(UserIdParameters args, Guid inconsistentEntityId)
            => await _busPublisher.SendRequest(new DeleteAccountingGroupUserAuthorization(inconsistentEntityId, args.UserId));
    }
}
