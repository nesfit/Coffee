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
    public class DeleteOrphanedManualStockOperationsActivity : ConsistencyRemediationActivity<StockItemIdParameters, Guid>
    {
        private readonly IStockOperationsService _stockOperationsService;
        private readonly IBusPublisher _busPublisher;

        public DeleteOrphanedManualStockOperationsActivity(IStockOperationsService stockOperationsService, IBusPublisher busPublisher, ILogger<DeleteOrphanedManualStockOperationsActivity> logger)
            : base(logger)
        {
            _stockOperationsService = stockOperationsService ?? throw new ArgumentNullException(nameof(stockOperationsService));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        protected override async Task<IEnumerable<Guid>> FindAsync(StockItemIdParameters args)
            => (await _stockOperationsService.BrowseManualStockOperations(new BrowseManualStockOperations() {StockItemId = new[] {args.StockItemId}})).Items.Select(i => i.Id);

        protected override async Task<IOperationResult> RemedyAsync(StockItemIdParameters args, Guid inconsistentEntityId)
            => await _busPublisher.SendRequest(new DeleteManualStockOperation(inconsistentEntityId));
    }
}
