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
    public class CreateSpendingLimitHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private static readonly Guid ParentUserAssignmentId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4776");
        private static readonly Guid ParentUserAssignmentMeansId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4777");
        private const decimal Value = 150;
        private static readonly TimeSpan Interval = TimeSpan.FromDays(1);

        private readonly Mock<ICreateSpendingLimit> _command;
        private ICreateSpendingLimit Cmd => _command.Object;

        public CreateSpendingLimitHandlerTests()
        {
            _command = new Mock<ICreateSpendingLimit>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.ParentUserAssignmentId).Returns(ParentUserAssignmentId);
            _command.SetupGet(c => c.Value).Returns(Value);
            _command.SetupGet(c => c.Interval).Returns(Interval);
        }

        private bool ValidateEquality(Domain.SpendingLimit spendingLimit)
        {
            Assert.AreEqual(Cmd.Id, spendingLimit.Id);
            Assert.AreEqual(Cmd.Interval, spendingLimit.Interval);
            Assert.AreEqual(Cmd.Value, spendingLimit.Value);
            return true;
        }

        private bool ValidateEquality(ISpendingLimitCreated @event)
        {
            Assert.AreEqual(Cmd.Id, @event.Id);
            Assert.AreEqual(Cmd.ParentUserAssignmentId, @event.ParentUserAssignmentId);
            Assert.AreEqual(Cmd.Interval, @event.Interval);
            Assert.AreEqual(Cmd.Value, @event.Value);
            return true;
        }

        [TestMethod]
        public void AddsSpendingLimitToRepository_RaisesIntegrationEvent()
        {
            var assignment = new Domain.AssignmentToUser(ParentUserAssignmentId, ParentUserAssignmentMeansId, DateTimeOffset.UtcNow, null, Guid.Empty, false);

            var repository = new Mock<IAssignmentsRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(ParentUserAssignmentId)).Returns(Task.FromResult<Domain.Assignment>(assignment)).Verifiable();
            repository.Setup(r => r.UpdateAsync(assignment)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<ISpendingLimitCreated>(It.Is<ISpendingLimitCreated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateSpendingLimitHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();

            ValidateEquality(assignment.SpendingLimits.Single());
        }
    }
}
