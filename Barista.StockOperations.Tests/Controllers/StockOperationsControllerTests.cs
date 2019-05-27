using Barista.Common.Dispatchers;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.StockOperations.Controllers;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.StockOperations.Tests.Controllers
{
    [TestClass]
    public class StockOperationsControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly StockOperationsController _controller;

        public StockOperationsControllerTests()
        {
            _controller = new StockOperationsController(_dispatcherMock.Object);
        }

        [TestMethod]
        public void GetStockOperation_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var stockItemId = TestIds.A;
            var result = new StockOperationDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetStockOperation>(q => q.Id == stockItemId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetStockOperation(stockItemId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetStockOperation_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var stockItemId = TestIds.A;
            var result = (StockOperationDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetStockOperation>(q => q.Id == stockItemId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetStockOperation(stockItemId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseStockOperations_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowseStockOperations();
            var result = new Mock<IPagedResult<StockOperationDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseStockOperations(query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }
    }
}
