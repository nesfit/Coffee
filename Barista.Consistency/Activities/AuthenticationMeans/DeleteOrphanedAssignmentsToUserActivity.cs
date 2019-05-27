using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Consistency.Commands;
using Barista.Consistency.Queries;
using Barista.Consistency.Services;
using Barista.Contracts.Commands.AssignmentToUser;
using Microsoft.Extensions.Logging;

namespace Barista.Consistency.Activities.AuthenticationMeans
{
    public class DeleteOrphanedAssignmentsToUserActivity : ConsistencyRemediationActivity<AuthenticationMeansIdParameters, Guid>
    {
        private readonly IIdentityService _identityService;
        private readonly IBusPublisher _busPublisher;

        public DeleteOrphanedAssignmentsToUserActivity(IBusPublisher busPublisher, IIdentityService identityService, ILogger<DeleteOrphanedAssignmentsToUserActivity> logger) : base(logger)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        protected override async Task<IEnumerable<Guid>> FindAsync(AuthenticationMeansIdParameters args)
            => (await _identityService.BrowseAssignmentsToUser(new BrowseAssignments() {OfAuthenticationMeans = args.AuthenticationMeansId})).Items.Select(i => i.Id);

        protected override async Task<IOperationResult> RemedyAsync(AuthenticationMeansIdParameters args, Guid inconsistentEntityId)
            => await _busPublisher.SendRequest(new DeleteAssignmentToUser(inconsistentEntityId));
    }
}
