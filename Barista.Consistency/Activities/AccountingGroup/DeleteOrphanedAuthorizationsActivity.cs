using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Consistency.Commands;
using Barista.Consistency.Services;
using Microsoft.Extensions.Logging;

namespace Barista.Consistency.Activities.AccountingGroup
{
    public class DeleteOrphanedAuthorizationsActivity : ConsistencyRemediationActivity<AccountingGroupIdParameters, Guid>
    {
        private readonly IBusPublisher _busPublisher;
        private readonly IAccountingGroupsService _agService;

        public DeleteOrphanedAuthorizationsActivity(IBusPublisher busPublisher, IAccountingGroupsService agService, ILogger<DeleteOrphanedAuthorizationsActivity> logger) : base(logger)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _agService = agService ?? throw new ArgumentNullException(nameof(agService));
        }

        protected override async Task<IEnumerable<Guid>> FindAsync(AccountingGroupIdParameters args)
            => (await _agService.BrowseAuthorizedUsers(args.AccountingGroupId)).Items.Select(au => au.UserId);


        protected override async Task<IOperationResult> RemedyAsync(AccountingGroupIdParameters args, Guid userId)
            => (await _busPublisher.SendRequest(new DeleteAccountingGroupUserAuthorization(args.AccountingGroupId, userId)));
    }
}
