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
    public class DeleteOrphanedPointOfSaleAuthorizationsActivity : ConsistencyRemediationActivity<UserIdParameters, Guid>
    {
        private readonly IPointsOfSaleService _posService;
        private readonly IBusPublisher _busPublisher;

        public DeleteOrphanedPointOfSaleAuthorizationsActivity(IPointsOfSaleService posService, IBusPublisher busPublisher, ILogger<DeleteOrphanedPointOfSaleAuthorizationsActivity> logger)
            : base(logger)
        {
            _posService = posService ?? throw new ArgumentNullException(nameof(posService));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        protected override async Task<IEnumerable<Guid>> FindAsync(UserIdParameters args)
            => (await _posService.BrowseUserAuthorizations(args.UserId, new PagedQuery())).Items.Select(i => i.PointOfSaleId);

        protected override async Task<IOperationResult> RemedyAsync(UserIdParameters args, Guid inconsistentEntityId)
            => await _busPublisher.SendRequest(new DeletePointOfSaleUserAuthorization(inconsistentEntityId, args.UserId));
    }
}
