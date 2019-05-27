using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.AssignmentToPointOfSale;
using Barista.Contracts.Events.AssignmentToPointOfSale;
using Barista.Identity.Handlers.AssignmentToPointOfSale;
using Barista.Identity.Repositories;
using Barista.Identity.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Identity.Tests.Handlers.AssignmentToPointOfSale
{
    [TestClass]
    public class UpdateAssignmentToPointOfSaleHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private static readonly Guid PointOfSaleId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4776");
        private static readonly Guid OfMeans = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4777");
        private static readonly DateTimeOffset ValidSince = new DateTimeOffset(2010, 1, 1, 1, 1, 1, TimeSpan.Zero);
        private static readonly DateTimeOffset ValidUntil = new DateTimeOffset(2020, 1, 1, 1, 1, 1, TimeSpan.Zero);

        private readonly Mock<IUpdateAssignmentToPointOfSale> _command;
        private IUpdateAssignmentToPointOfSale Cmd => _command.Object;
        public UpdateAssignmentToPointOfSaleHandlerTests()
        {
            _command = new Mock<IUpdateAssignmentToPointOfSale>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.PointOfSaleId).Returns(PointOfSaleId);
            _command.SetupGet(c => c.MeansId).Returns(OfMeans);
            _command.SetupGet(c => c.ValidSince).Returns(ValidSince);
            _command.SetupGet(c => c.ValidUntil).Returns(ValidUntil);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.PointOfSaleId, domainObjectOrEvent.PointOfSaleId);
            Assert.AreEqual(Cmd.MeansId, domainObjectOrEvent.MeansId);
            Assert.AreEqual(Cmd.ValidSince, domainObjectOrEvent.ValidSince);
            Assert.AreEqual(Cmd.ValidUntil, domainObjectOrEvent.ValidUntil);
            return true;
        }

        [TestMethod]
        public void UpdatesAssignmentInRepository_RaisesIntegrationEvent()
        {
            var assignment = new Domain.AssignmentToPointOfSale(Id, Guid.Empty, DateTimeOffset.UtcNow, null, Guid.Empty);

            var repository = new Mock<IAssignmentsRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(Id)).Returns(Task.FromResult<Domain.Assignment>(assignment)).Verifiable();
            repository.Setup(r => r.UpdateAsync(assignment)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IAssignmentToPointOfSaleUpdated>(It.Is<IAssignmentToPointOfSaleUpdated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var meansVerifier = new Mock<IAuthenticationMeansVerifier>(MockBehavior.Strict);
            meansVerifier.Setup(v => v.AssertExists(OfMeans)).Returns(Task.CompletedTask).Verifiable();

            var posVerifier = new Mock<IPointOfSaleVerifier>(MockBehavior.Strict);
            posVerifier.Setup(v => v.AssertExists(PointOfSaleId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new UpdateAssignmentToPointOfSaleHandler(repository.Object, busPublisher.Object, meansVerifier.Object, posVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            ValidateEquality(assignment);
            repository.Verify();
            busPublisher.Verify();
            meansVerifier.Verify();
            posVerifier.Verify();
        }
    }
}
