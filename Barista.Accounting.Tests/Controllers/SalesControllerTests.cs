using Microsoft.VisualStudio.TestTools.UnitTesting;
using Barista.Accounting.Controllers;
using Barista.Accounting.Dto;
using Barista.Common.Dispatchers;
using Moq;
using Barista.Common.Tests;
using Barista.Accounting.Queries;
using Barista.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Accounting.Tests.Controllers
{
    [TestClass]
    public class SalesControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly SalesController _controller;

        public SalesControllerTests() => _controller = new SalesController(_dispatcherMock.Object);

        [TestMethod]
        public void GetSale_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var saleId = TestIds.A;
            var resultDto = new Dto.SaleDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetSale>(q => q.Id == saleId))).ReturnsAsync(resultDto).Verifiable();

            var actionResult = _controller.GetSale(saleId).GetAwaiter().GetResult();

            Assert.AreEqual(resultDto, actionResult.Value);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetSale_ConstructsQuery_WhenDispatchResultIsNull_ReturnsNotFound()
        {
            var saleId = TestIds.A;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetSale>(q => q.Id == saleId))).ReturnsAsync((SaleDto)null).Verifiable();

            var actionResult = _controller.GetSale(saleId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseSales_UsesQuery_ReturnsResultOfDispatch()
        {
            var result = new Mock<IPagedResult<Dto.SaleDto>>(MockBehavior.Strict).Object;
            var query = new BrowseSales();
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var returnedResult = _controller.BrowseSales(query).GetAwaiter().GetResult();

            Assert.AreEqual(result, returnedResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetSaleStateChange_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var saleId = TestIds.A;
            var changeId = TestIds.B;
            var resultDto = new Dto.SaleStateChangeDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetSaleStateChange>(q => q.ParentSaleId == saleId && q.SaleStateChangeId == changeId))).ReturnsAsync(resultDto).Verifiable();

            var actionResult = _controller.GetSaleStateChange(parentSaleId: saleId, saleStateChangeId: changeId).GetAwaiter().GetResult();

            Assert.AreEqual(resultDto, actionResult.Value);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetSaleStateChange_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var saleId = TestIds.A;
            var changeId = TestIds.B;
            var resultDto = new Dto.SaleStateChangeDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetSaleStateChange>(q => q.ParentSaleId == saleId && q.SaleStateChangeId == changeId))).ReturnsAsync((Dto.SaleStateChangeDto)null).Verifiable();

            var actionResult = _controller.GetSaleStateChange(parentSaleId: saleId, saleStateChangeId: changeId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseSaleStateChanges_UsesQuery_WithEnforcedParentSaleId_ReturnsResultOfDispatch()
        {
            var parentSaleId = TestIds.A;
            var query = new BrowseSaleStateChanges(TestIds.B);
            var result = new Mock<IPagedResult<SaleStateChangeDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<BrowseSaleStateChanges>(q => q == query && q.ParentSaleId == parentSaleId))).ReturnsAsync(result).Verifiable();

            Assert.AreEqual(result, _controller.BrowseSaleStateChanges(parentSaleId, query).GetAwaiter().GetResult());
            _dispatcherMock.Verify();
        }
    }
}
