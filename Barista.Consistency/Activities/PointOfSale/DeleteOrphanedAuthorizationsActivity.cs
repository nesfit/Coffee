using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Consistency.Commands;
using Barista.Consistency.Services;
using Microsoft.Extensions.Logging;

namespace Barista.Consistency.Activities.PointOfSale
{
    public class DeleteOrphanedAuthorizationsActivity : ConsistencyRemediationActivity<PointOfSaleIdParameters, Guid>
    {
        private readonly IBusPublisher _busPublisher;
        private readonly IPointsOfSaleService _posService;

        public DeleteOrphanedAuthorizationsActivity(IBusPublisher busPublisher, IPointsOfSaleService posService, ILogger<DeleteOrphanedAuthorizationsActivity> logger) : base(logger)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _posService = posService ?? throw new ArgumentNullException(nameof(posService));
        }

        protected override async Task<IEnumerable<Guid>> FindAsync(PointOfSaleIdParameters args)
            => (await _posService.BrowseAuthorizedUsers(args.PointOfSaleId)).Items.Select(i => i.UserId);

        protected override async Task<IOperationResult> RemedyAsync(PointOfSaleIdParameters args, Guid inconsistentEntityId)
            => await _busPublisher.SendRequest(new DeletePointOfSaleUserAuthorization(args.PointOfSaleId, inconsistentEntityId));
    }
}
