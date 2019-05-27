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
    public class SaleBasedStockOperationsControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly SaleBasedStockOperationsController _controller;

        public SaleBasedStockOperationsControllerTests()
        {
            _controller = new SaleBasedStockOperationsController(_dispatcherMock.Object);
        }

        [TestMethod]
        public void GetSaleBasedStockOperation_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var stockItemId = TestIds.A;
            var result = new SaleBasedStockOperationDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetSaleBasedStockOperation>(q => q.Id == stockItemId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetSaleBasedStockOperation(stockItemId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetSaleBasedStockOperation_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var stockItemId = TestIds.A;
            var result = (SaleBasedStockOperationDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetSaleBasedStockOperation>(q => q.Id == stockItemId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetSaleBasedStockOperation(stockItemId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseSaleBasedStockOperations_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowseSaleBasedStockOperations();
            var result = new Mock<IPagedResult<SaleBasedStockOperationDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseSaleBasedStockOperations(query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }
    }
}
