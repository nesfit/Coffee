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

namespace Barista.Consistency.Activities.StockItem
{
    public class UnsetOfferReferenceActivity : ConsistencyRemediationActivity<StockItemIdParameters, Guid>
    {
        private readonly IOffersService _offersService;
        private readonly IBusPublisher _busPublisher;

        public UnsetOfferReferenceActivity(IOffersService offersService, IBusPublisher busPublisher, ILogger<UnsetOfferReferenceActivity> logger) : base(logger)
        {
            _offersService = offersService ?? throw new ArgumentNullException(nameof(offersService));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        protected override async Task<IEnumerable<Guid>> FindAsync(StockItemIdParameters args)
            => (await _offersService.BrowseOffers(new BrowseOffers() {OfStockItemId = args.StockItemId})).Items.Select(i => i.Id);

        protected override async Task<IOperationResult> RemedyAsync(StockItemIdParameters args, Guid inconsistentEntityId)
            => await _busPublisher.SendRequest(new UnsetOfferStockItemReference(inconsistentEntityId, args.StockItemId));
    }
}
