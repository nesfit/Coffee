using Barista.Common.Dispatchers;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.StockItems.Controllers;
using Barista.StockItems.Dto;
using Barista.StockItems.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.StockItems.Tests.Controllers
{
    [TestClass]
    public class StockItemsControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly StockItemsController _controller;

        public StockItemsControllerTests()
        {
            _controller = new StockItemsController(_dispatcherMock.Object);
        }

        [TestMethod]
        public void GetStockItem_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var stockItemId = TestIds.A;
            var result = new StockItemDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetStockItem>(q => q.Id == stockItemId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetStockItem(stockItemId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetStockItem_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var stockItemId = TestIds.A;
            var result = (StockItemDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetStockItem>(q => q.Id == stockItemId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetStockItem(stockItemId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseStockItems_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowseStockItems();
            var result = new Mock<IPagedResult<StockItemDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseStockItems(query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }
    }
}
