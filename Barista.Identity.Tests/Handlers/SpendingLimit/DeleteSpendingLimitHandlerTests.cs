using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.AuthenticationMeans;
using Barista.Contracts.Commands.SpendingLimit;
using Barista.Contracts.Events.AuthenticationMeans;
using Barista.Contracts.Events.SpendingLimit;
using Barista.Identity.Handlers.AuthenticationMeans;
using Barista.Identity.Handlers.SpendingLimit;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Identity.Tests.Handlers.SpendingLimit
{
    [TestClass]
    public class DeleteSpendingLimitHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private static readonly Guid ParentUserAssignmentId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4776");
        private static readonly Guid ParentUserAssignmentMeansId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4777");

        private readonly Mock<IDeleteSpendingLimit> _command;
        private IDeleteSpendingLimit Cmd => _command.Object;

        public DeleteSpendingLimitHandlerTests()
        {
            _command = new Mock<IDeleteSpendingLimit>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.ParentUserAssignmentId).Returns(ParentUserAssignmentId);
        }

        private bool ValidateEquality(ISpendingLimitDeleted @event)
        {
            Assert.AreEqual(Cmd.Id, @event.Id);
            Assert.AreEqual(Cmd.ParentUserAssignmentId, @event.ParentUserAssignmentId);
            return true;
        }

        [TestMethod]
        public void UpdatesSpendingLimitInRepository_RaisesIntegrationEvent()
        {
            var assignment = new Domain.AssignmentToUser(ParentUserAssignmentId, ParentUserAssignmentMeansId, DateTimeOffset.UtcNow, null, Guid.Empty, false);
            var spendingLimit = new Domain.SpendingLimit(Id, TimeSpan.FromMinutes(2), 111);
            assignment.SpendingLimits.Add(spendingLimit);

            var repository = new Mock<IAssignmentsRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(ParentUserAssignmentId)).Returns(Task.FromResult<Domain.Assignment>(assignment)).Verifiable();
            repository.Setup(r => r.UpdateAsync(assignment)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<ISpendingLimitDeleted>(It.Is<ISpendingLimitDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteSpendingLimitHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
            Assert.IsFalse(assignment.SpendingLimits.Any());
        }
    }
}
