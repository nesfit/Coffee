using Barista.AccountingGroups.Controllers;
using Barista.AccountingGroups.Dto;
using Barista.AccountingGroups.Queries;
using Barista.Common.Dispatchers;
using Barista.Common.Tests;
using Barista.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Barista.AccountingGroups.Tests.Controllers
{
    [TestClass]
    public class AccountingGroupsControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly AccountingGroupsController _controller;

        public AccountingGroupsControllerTests() => _controller = new AccountingGroupsController(_dispatcherMock.Object);

        [TestMethod]
        public void GetAccountingGroup_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var agId = TestIds.A;
            var result = new AccountingGroupDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetAccountingGroup>(q => q.Id == agId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetAccountingGroup(agId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetAccountingGroup_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var agId = TestIds.A;
            var result = (AccountingGroupDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetAccountingGroup>(q => q.Id == agId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetAccountingGroup(agId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseAccountingGroups_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowseAccountingGroups();
            var result = new Mock<IPagedResult<AccountingGroupDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseAccountingGroups(query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseAuthorizedUsers_UsesQuery_WithEnforcedAccountingGroupId_ReturnsResultOfDispatch()
        {
            var agId = TestIds.A;
            var query = new BrowseUserAuthorizations { AccountingGroupId = TestIds.B };
            var result = new Mock<IPagedResult<UserAuthorizationDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<BrowseUserAuthorizations>(q => q == query && q.AccountingGroupId == agId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseAuthorizedUsers(agId, query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetUserAuthorization_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var agId = TestIds.A;
            var userId = TestIds.B;            
            var result = new UserAuthorizationDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetUserAuthorization>(q => q.AccountingGroupId == agId && q.UserId == userId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetUserAuthorization(agId, userId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetUserAuthorization_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var agId = TestIds.A;
            var userId = TestIds.B;
            var result = (UserAuthorizationDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetUserAuthorization>(q => q.AccountingGroupId == agId && q.UserId == userId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetUserAuthorization(agId, userId).GetAwaiter().GetResult();

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

            var actionResult = _controller.BrowseUserAuthorizations(userId, query).GetAwaiter().GetResult();
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

        [TestMethod]
        public void FindAuthorizedUser_ConstructsQuery_WhenUserHasAtLeastOneAuthorization_ReturnsOk()
        {
            var userId = TestIds.A;
            var resultMock = new Mock<IPagedResult<UserAuthorizationDto>>(MockBehavior.Strict);
            resultMock.SetupGet(r => r.TotalResults).Returns(1);
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<BrowseUserAuthorizations>(q => q.UserId == userId))).ReturnsAsync(resultMock.Object).Verifiable();

            var actionResult = _controller.FindAuthorizedUser(userId).GetAwaiter().GetResult();
            Assert.IsTrue(actionResult is OkResult);

            _dispatcherMock.Verify();
        }
    }
}
