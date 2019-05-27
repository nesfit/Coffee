using Barista.Common.Tests;
using Barista.SaleStrategies.Controllers;
using Barista.SaleStrategies.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Barista.SaleStrategies.Queries;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Barista.SaleStrategies.Tests.Controllers
{
    [TestClass]
    public class SaleStrategiesControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly SaleStrategiesController _controller;

        public SaleStrategiesControllerTests()
        {
            _controller = new SaleStrategiesController(_dispatcherMock.Object);
        }

        [TestMethod]
        public void GetSaleStrategy_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var ssId = TestIds.A;
            var result = new SaleStrategyDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetSaleStrategy>(q => q.Id == ssId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetSaleStrategy(ssId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetSaleStrategy_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var ssId = TestIds.A;
            var result = (SaleStrategyDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetSaleStrategy>(q => q.Id == ssId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetSaleStrategy(ssId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseSaleStrategies_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowseSaleStrategies();
            var result = new Mock<IPagedResult<SaleStrategyDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseSaleStrategies(query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }
    }
}
