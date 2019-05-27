using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Handlers.StockOperation;
using Barista.StockOperations.Queries;
using Barista.StockOperations.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.StockOperations.Tests.Handlers.StockOperation
{
    [TestClass]
    public class BrowseStockOperationsHandlerTests : BrowseHandlerTestBase<BrowseStockOperations, IStockOperationRepository, Domain.StockOperation, StockOperationDto>
    {
        protected override IQueryHandler<BrowseStockOperations, IPagedResult<StockOperationDto>> InstantiateHandler()
            => new BrowseStockOperationsHandler(Repository, Mapper);
    }
}
