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
    public class AssignmentsToPointOfSaleControllerTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly AssignmentsToPointOfSaleController _controller;

        public AssignmentsToPointOfSaleControllerTests() => _controller = new AssignmentsToPointOfSaleController(_dispatcherMock.Object);

        [TestMethod]
        public void GetAssignment_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var assignmentId = TestIds.A;
            var result = new AssignmentToPointOfSaleDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetAssignmentToPointOfSale>(q => q.Id == assignmentId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetAssignment(assignmentId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetAssignment_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var assignmentId = TestIds.A;
            var result = (AssignmentToPointOfSaleDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetAssignmentToPointOfSale>(q => q.Id == assignmentId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetAssignment(assignmentId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseAssignments_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowseAssignmentsToPointOfSale();
            var result = new Mock<IPagedResult<AssignmentToPointOfSaleDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseAssignments(query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }
    }
}
