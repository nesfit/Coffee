using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.StockItems.Dto;
using Barista.StockItems.Handlers.StockItem;
using Barista.StockItems.Queries;
using Barista.StockItems.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.StockItems.Tests.Handlers.StockItem
{
    [TestClass]
    public class BrowseStockItemsHandlerTests : BrowseHandlerTestBase<BrowseStockItems, IStockItemRepository, Domain.StockItem, StockItemDto>
    {
        protected override IQueryHandler<BrowseStockItems, IPagedResult<StockItemDto>> InstantiateHandler()
            => new BrowseStockItemsHandler(Repository, Mapper);
    }
}
