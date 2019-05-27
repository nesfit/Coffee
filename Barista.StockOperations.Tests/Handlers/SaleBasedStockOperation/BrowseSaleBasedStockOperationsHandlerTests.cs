using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Handlers.SaleBasedStockOperation;
using Barista.StockOperations.Queries;
using Barista.StockOperations.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.StockOperations.Tests.Handlers.SaleBasedStockOperation
{
    [TestClass]
    public class BrowseSaleBasedStockOperationsHandlerTests : BrowseHandlerTestBase<BrowseSaleBasedStockOperations, IStockOperationRepository, Domain.SaleBasedStockOperation, SaleBasedStockOperationDto>
    {
        protected override IQueryHandler<BrowseSaleBasedStockOperations, IPagedResult<SaleBasedStockOperationDto>> InstantiateHandler()
            => new BrowseSaleBasedStockOperationsHandler(Repository, Mapper);
    }
}
