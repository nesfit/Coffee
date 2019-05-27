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
    public class ManualStockOperationsControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly ManualStockOperationsController _controller;

        public ManualStockOperationsControllerTests()
        {
            _controller = new ManualStockOperationsController(_dispatcherMock.Object);
        }

        [TestMethod]
        public void GetManualStockOperation_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var stockItemId = TestIds.A;
            var result = new ManualStockOperationDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetManualStockOperation>(q => q.Id == stockItemId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetManualStockOperation(stockItemId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetManualStockOperation_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var stockItemId = TestIds.A;
            var result = (ManualStockOperationDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetManualStockOperation>(q => q.Id == stockItemId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetManualStockOperation(stockItemId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseManualStockOperations_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowseManualStockOperations();
            var result = new Mock<IPagedResult<ManualStockOperationDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseManualStockOperations(query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }
    }
}
