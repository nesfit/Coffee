using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.AssignmentToPointOfSale;
using Barista.Contracts.Commands.AssignmentToUser;
using Barista.Contracts.Events.AssignmentToPointOfSale;
using Barista.Contracts.Events.AssignmentToUser;
using Barista.Identity.Handlers.AssignmentToPointOfSale;
using Barista.Identity.Handlers.AssignmentToUser;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Identity.Tests.Handlers.AssignmentToUser
{
    [TestClass]
    public class DeleteAssignmentToUserHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");

        private readonly Mock<IDeleteAssignmentToUser> _command;
        private IDeleteAssignmentToUser Cmd => _command.Object;

        public DeleteAssignmentToUserHandlerTests()
        {
            _command = new Mock<IDeleteAssignmentToUser>();
            _command.SetupGet(c => c.Id).Returns(Id);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            return true;
        }

        [TestMethod]
        public void UpdatesAssignmentInRepository_RaisesIntegrationEvent()
        {
            var assignment = new Domain.AssignmentToUser(Id, Guid.Empty, DateTimeOffset.UtcNow, null, Guid.Empty, false);

            var repository = new Mock<IAssignmentsRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(Id)).Returns(Task.FromResult<Domain.Assignment>(assignment)).Verifiable();
            repository.Setup(r => r.DeleteAsync(assignment)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IAssignmentToUserDeleted>(It.Is<IAssignmentToUserDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteAssignmentToUserHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
