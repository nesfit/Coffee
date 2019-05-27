using Barista.Common.Dispatchers;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.Identity.Controllers;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Barista.Identity.Tests.Controllers
{
    [TestClass]
    public class AuthenticationMeansControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly AuthenticationMeansController _controller;

        public AuthenticationMeansControllerTests() => _controller = new AuthenticationMeansController(_dispatcherMock.Object);

        [TestMethod]
        public void GetAuthenticationMeans_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var meansId = TestIds.A;
            var result = new AuthenticationMeansDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetAuthenticationMeans>(q => q.Id == meansId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetAuthenticationMeans(meansId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetAuthenticationMeans_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var meansId = TestIds.A;
            var result = (AuthenticationMeansDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetAuthenticationMeans>(q => q.Id == meansId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetAuthenticationMeans(meansId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseAuthenticationMeans_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowseAuthenticationMeans();
            var result = new Mock<IPagedResult<AuthenticationMeansDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseAuthenticationMeans(query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseValidUserPasswords_UsesQuery_EnforcesUserId_ReturnsResultOfDispatch()
        {
            var userId = TestIds.A;
            var query = new BrowseValidUserPasswords() { UserId = TestIds.B };
            var result = new Mock<IPagedResult<AuthenticationMeansWithValueDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<BrowseValidUserPasswords>(q => q == query && q.UserId == userId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseValidPasswords(userId, query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }
    }
}
