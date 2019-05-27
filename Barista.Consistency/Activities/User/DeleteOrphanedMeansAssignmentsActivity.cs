using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Consistency.Commands;
using Barista.Consistency.Queries;
using Barista.Consistency.Services;
using Microsoft.Extensions.Logging;

namespace Barista.Consistency.Activities.User
{
    public class DeleteOrphanedMeansAssignmentsActivity : ConsistencyRemediationActivity<UserIdParameters, Guid>
    {
        private readonly IIdentityService _identityService;
        private readonly IBusPublisher _busPublisher;

        public DeleteOrphanedMeansAssignmentsActivity(IIdentityService identityService, IBusPublisher busPublisher, ILogger<DeleteOrphanedMeansAssignmentsActivity> logger) : base(logger)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        protected override async Task<IEnumerable<Guid>> FindAsync(UserIdParameters args)
            => (await _identityService.BrowseAssignmentsToUser(new BrowseAssignments() {AssignedToUser = args.UserId})).Items.Select(i => i.Id);

        protected override async Task<IOperationResult> RemedyAsync(UserIdParameters args, Guid inconsistentEntityId)
            => await _busPublisher.SendRequest(new DeleteAssignmentToUser(inconsistentEntityId));
    }
}
