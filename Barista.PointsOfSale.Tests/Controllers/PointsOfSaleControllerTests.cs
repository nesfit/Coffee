using Barista.Common.Dispatchers;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.PointsOfSale.Controllers;
using Barista.PointsOfSale.Dto;
using Barista.PointsOfSale.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Barista.PointsOfSale.Tests.Controllers
{
    [TestClass]
    public class PointsOfSaleControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly PointsOfSaleController _controller;

        public PointsOfSaleControllerTests()
        {
            _controller = new PointsOfSaleController(_dispatcherMock.Object);
        }

        [TestMethod]
        public void GetPointOfSale_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var posId = TestIds.A;
            var result = new PointOfSaleDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetPointOfSale>(q => q.Id == posId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetPointOfSale(posId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetPointOfSale_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var posId = TestIds.A;
            var result = (PointOfSaleDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetPointOfSale>(q => q.Id == posId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetPointOfSale(posId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowsePointsOfSale_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowsePointsOfSale();
            var result = new Mock<IPagedResult<PointOfSaleDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowsePointsOfSale(query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseAuthorizedUsers_UsesQuery_WithEnforcedPointOfSaled_ReturnsResultOfDispatch()
        {
            var posId = TestIds.A;
            var query = new BrowseUserAuthorizations { PointOfSaleId = TestIds.B };
            var result = new Mock<IPagedResult<UserAuthorizationDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<BrowseUserAuthorizations>(q => q == query && q.PointOfSaleId == posId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseAuthorizedUsers(posId, query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetUserAuthorization_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var posId = TestIds.A;
            var userId = TestIds.B;
            var result = new UserAuthorizationDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetUserAuthorization>(q => q.PointOfSaleId == posId && q.UserId == userId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetUserAuthorization(posId, userId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetUserAuthorization_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var posId = TestIds.A;
            var userId = TestIds.B;
            var result = (UserAuthorizationDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetUserAuthorization>(q => q.PointOfSaleId == posId && q.UserId == userId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetUserAuthorization(posId, userId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseUserAuthorizations_UsesQuery_WithEnforcedUserId_ReturnsResultOfDispatch()
        {
            var userId = TestIds.A;
            var query = new BrowseUserAuthorizations { UserId = TestIds.B };
            var result = new Mock<IPagedResult<UserAuthorizationDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<BrowseUserAuthorizations>(q => q == query && q.UserId == userId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseUserAuthorizations(query, userId).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void FindAuthorizedUser_ConstructsQuery_WhenUserHasNoAuthorizations_ReturnsNoContent()
        {
            var userId = TestIds.A;
            var resultMock = new Mock<IPagedResult<UserAuthorizationDto>>(MockBehavior.Strict);
            resultMock.SetupGet(r => r.TotalResults).Returns(0);
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<BrowseUserAuthorizations>(q => q.UserId == userId))).ReturnsAsync(resultMock.Object).Verifiable();

            var actionResult = _controller.FindAuthorizedUser(userId).GetAwaiter().GetResult();
            Assert.IsTrue(actionResult is NoContentResult);

            _dispatcherMock.Verify();
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void FindAuthorizedUser_ConstructsQuery_WhenUserHasAtLeastOneAuthorization_ReturnsOk(int userAuthCount)
        {
            var userId = TestIds.A;
            var resultMock = new Mock<IPagedResult<UserAuthorizationDto>>(MockBehavior.Strict);
            resultMock.SetupGet(r => r.TotalResults).Returns(userAuthCount);
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<BrowseUserAuthorizations>(q => q.UserId == userId))).ReturnsAsync(resultMock.Object).Verifiable();

            var actionResult = _controller.FindAuthorizedUser(userId).GetAwaiter().GetResult();
            Assert.IsTrue(actionResult is OkResult);

            _dispatcherMock.Verify();
        }
    }
}
