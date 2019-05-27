using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Handlers.ManualStockOperation;
using Barista.StockOperations.Queries;
using Barista.StockOperations.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.StockOperations.Tests.Handlers.ManualStockOperation
{
    [TestClass]
    public class BrowseManualStockOperationsHandlerTests : BrowseHandlerTestBase<BrowseManualStockOperations, IStockOperationRepository, Domain.ManualStockOperation, ManualStockOperationDto>
    {
        protected override IQueryHandler<BrowseManualStockOperations, IPagedResult<ManualStockOperationDto>> InstantiateHandler()
            => new BrowseManualStockOperationsHandler(Repository, Mapper);
    }
}
