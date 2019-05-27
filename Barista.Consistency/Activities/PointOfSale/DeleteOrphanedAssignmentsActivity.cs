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

namespace Barista.Consistency.Activities.PointOfSale
{
    public class DeleteOrphanedAssignmentsActivity : ConsistencyRemediationActivity<PointOfSaleIdParameters, Guid>
    {
        private readonly IIdentityService _identityService;
        private readonly IBusPublisher _busPublisher;

        public DeleteOrphanedAssignmentsActivity(IIdentityService identityService, IBusPublisher busPublisher, ILogger<DeleteOrphanedAssignmentsActivity> logger) : base(logger)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        protected override async Task<IEnumerable<Guid>> FindAsync(PointOfSaleIdParameters args)
            => (await _identityService.BrowseAssignmentsToPointOfSale(new BrowseAssignments() {AssignedToPointOfSale = args.PointOfSaleId})).Items.Select(i => i.Id);

        protected override async Task<IOperationResult> RemedyAsync(PointOfSaleIdParameters args, Guid inconsistentEntityId)
            => await _busPublisher.SendRequest(new DeleteAssignmentToPointOfSale(inconsistentEntityId));
        
    }
}
