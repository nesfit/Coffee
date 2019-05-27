using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.AssignmentToPointOfSale;
using Barista.Contracts.Events.AssignmentToPointOfSale;
using Barista.Identity.Handlers.AssignmentToPointOfSale;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Identity.Tests.Handlers.AssignmentToPointOfSale
{
    [TestClass]
    public class DeleteAssignmentToPointOfSaleHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");

        private readonly Mock<IDeleteAssignmentToPointOfSale> _command;
        private IDeleteAssignmentToPointOfSale Cmd => _command.Object;

        public DeleteAssignmentToPointOfSaleHandlerTests()
        {
            _command = new Mock<IDeleteAssignmentToPointOfSale>();
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
            var assignment = new Domain.AssignmentToPointOfSale(Id, Guid.Empty, DateTimeOffset.UtcNow, null, Guid.Empty);

            var repository = new Mock<IAssignmentsRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(Id)).Returns(Task.FromResult<Domain.Assignment>(assignment)).Verifiable();
            repository.Setup(r => r.DeleteAsync(assignment)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IAssignmentToPointOfSaleDeleted>(It.Is<IAssignmentToPointOfSaleDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteAssignmentToPointOfSaleHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
