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
    public class PaymentsControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly PaymentsController _controller;

        public PaymentsControllerTests() => _controller = new PaymentsController(_dispatcherMock.Object);

        [TestMethod]
        public void GetPayment_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var resultDto = new Dto.PaymentDto();
            var query = new GetPayment(TestIds.A);
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(resultDto).Verifiable();

            var actionResult = _controller.GetPayment(query).GetAwaiter().GetResult();

            Assert.AreEqual(resultDto, actionResult.Value);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetPayment_ConstructsQuery_WhenDispatchResultIsNull_ReturnsNotFound()
        {
            var query = new GetPayment(TestIds.A);
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync((PaymentDto)null).Verifiable();

            var actionResult = _controller.GetPayment(query).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowsePayments_UsesQuery_ReturnsResultOfDispatch()
        {
            var result = new Mock<IPagedResult<Dto.PaymentDto>>(MockBehavior.Strict).Object;
            var query = new BrowsePayments();
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var returnedResult = _controller.BrowsePayments(query).GetAwaiter().GetResult();

            Assert.AreEqual(result, returnedResult);
            _dispatcherMock.Verify();
        }
    }
}
