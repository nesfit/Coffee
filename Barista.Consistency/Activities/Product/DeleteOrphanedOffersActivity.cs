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

namespace Barista.Consistency.Activities.Product
{
    public class DeleteOrphanedOffersActivity : ConsistencyRemediationActivity<ProductIdParameters, Guid>
    {
        private readonly IBusPublisher _busPublisher;
        private readonly IOffersService _offersService;

        public DeleteOrphanedOffersActivity(IBusPublisher busPublisher, IOffersService offersService, ILogger<DeleteOrphanedOffersActivity> logger) : base(logger)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _offersService = offersService ?? throw new ArgumentNullException(nameof(offersService));
        }

        protected override async Task<IEnumerable<Guid>> FindAsync(ProductIdParameters args)
            => (await _offersService.BrowseOffers(new BrowseOffers() { OfProductId = args.ProductId })).Items.Select(o => o.Id);

        protected override async Task<IOperationResult> RemedyAsync(ProductIdParameters args, Guid inconsistentEntityId)
            => await _busPublisher.SendRequest(new DeleteOffer(inconsistentEntityId));
    }
}
