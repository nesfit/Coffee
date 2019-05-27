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
    public class AssignmentsControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly AssignmentsController _controller;

        public AssignmentsControllerTests() => _controller = new AssignmentsController(_dispatcherMock.Object);

        [TestMethod]
        public void GetAssignment_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var assignmentId = TestIds.A;
            var result = new AssignmentDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetAssignment>(q => q.Id == assignmentId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetAssignment(assignmentId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetAssignment_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var assignmentId = TestIds.A;
            var result = (AssignmentDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetAssignment>(q => q.Id == assignmentId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetAssignment(assignmentId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseAssignments_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowseAssignments();
            var result = new Mock<IPagedResult<AssignmentDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseAssignments(query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }
    }
}
