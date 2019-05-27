using Barista.Common.Dispatchers;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.Users.Controllers;
using Barista.Users.Dto;
using Barista.Users.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Barista.Users.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _controller = new UsersController(_dispatcherMock.Object);
        }

        [TestMethod]
        public void GetUser_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var userId = TestIds.A;
            var result = new UserDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetUser>(q => q.Id == userId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetUser(userId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetUser_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var userId = TestIds.A;
            var result = (UserDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetUser>(q => q.Id == userId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetUser(userId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseUsers_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowseUsers();
            var result = new Mock<IPagedResult<UserDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseUsers(query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }
    }
}
