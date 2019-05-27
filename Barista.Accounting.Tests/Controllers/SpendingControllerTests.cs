using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Barista.Accounting.Controllers;
using Barista.Common.Dispatchers;
using Moq;
using Barista.Common.Tests;
using Barista.Accounting.Queries;

namespace Barista.Accounting.Tests.Controllers
{
    [TestClass]
    public class SpendingControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly SpendingController _controller;

        public SpendingControllerTests() => _controller = new SpendingController(_dispatcherMock.Object);

        [TestMethod]
        public void GetSpendingOfAuthenticationMeans_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var meansId = TestIds.A;
            var expectedResult = 123m;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetSpendingOfMeans>(q => q.AuthenticationMeansId == meansId && q.Since == null))).ReturnsAsync(expectedResult).Verifiable();

            var actionResult = _controller.GetSpendingOfAuthenticationMeans(meansId).GetAwaiter().GetResult();

            Assert.AreEqual(expectedResult, actionResult.Value);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetSpendingOfAuthenticationMeans_WithSince_ConstructsCorrectQuery_ReturnsResultOfDispatch()
        {
            var meansId = TestIds.A;
            DateTimeOffset since = TestDateTimes.Year2001;
            var expectedResult = 123m;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetSpendingOfMeans>(q => q.AuthenticationMeansId == meansId && q.Since == since))).ReturnsAsync(expectedResult).Verifiable();

            var actionResult = _controller.GetSpendingOfAuthenticationMeans(meansId, since).GetAwaiter().GetResult();

            Assert.AreEqual(expectedResult, actionResult.Value);
            _dispatcherMock.Verify();
        }
    }
}
