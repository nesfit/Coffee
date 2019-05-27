using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Barista.Accounting.Controllers;
using Barista.Common.Dispatchers;
using Moq;
using Barista.Common.Tests;
using Barista.Accounting.Queries;

namespace Barista.Accounting.Tests.Controllers
{
    [TestClass]
    public class BalanceControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly BalanceController _controller;

        public BalanceControllerTests() => _controller = new BalanceController(_dispatcherMock.Object);

        [TestMethod]
        public void GetUserBalance_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var resultDto = new Dto.BalanceDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetBalance>(q => q.UserId == TestIds.A))).ReturnsAsync(resultDto).Verifiable();

            var actionResult = _controller.GetUserBalance(TestIds.A).GetAwaiter().GetResult();

            Assert.AreEqual(resultDto, actionResult.Value);
            _dispatcherMock.Verify();
        }
    }
}
