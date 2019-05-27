using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.AssignmentToUser;
using Barista.Contracts.Events.AssignmentToUser;
using Barista.Identity.Handlers.AssignmentToUser;
using Barista.Identity.Repositories;
using Barista.Identity.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Identity.Tests.Handlers.AssignmentToUser
{
    [TestClass]
    public class CreateAssignmentToUserHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private static readonly Guid UserId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4776");
        private static readonly Guid MeansId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4777");
        private static readonly DateTimeOffset ValidSince = new DateTimeOffset(2010, 1, 1, 1, 1, 1, TimeSpan.Zero);
        private static readonly DateTimeOffset ValidUntil = new DateTimeOffset(2020, 1, 1, 1, 1, 1, TimeSpan.Zero);
        private const bool IsShared = false;

        private readonly Mock<ICreateAssignmentToUser> _command;
        private ICreateAssignmentToUser Cmd => _command.Object;


        public CreateAssignmentToUserHandlerTests()
        {
            _command = new Mock<ICreateAssignmentToUser>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.UserId).Returns(UserId);
            _command.SetupGet(c => c.MeansId).Returns(MeansId);
            _command.SetupGet(c => c.ValidSince).Returns(ValidSince);
            _command.SetupGet(c => c.ValidUntil).Returns(ValidUntil);
            _command.SetupGet(c => c.IsShared).Returns(IsShared);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.UserId, domainObjectOrEvent.UserId);
            Assert.AreEqual(Cmd.MeansId, domainObjectOrEvent.MeansId);
            Assert.AreEqual(Cmd.ValidSince, domainObjectOrEvent.ValidSince);
            Assert.AreEqual(Cmd.ValidUntil, domainObjectOrEvent.ValidUntil);
            Assert.AreEqual(Cmd.IsShared, domainObjectOrEvent.IsShared);
            return true;
        }

        [TestMethod]
        public void AddsAssignmentToRepository_RaisesIntegrationEvent()
        {
            var repository = new Mock<IAssignmentsRepository>(MockBehavior.Strict);
            repository.Setup(r => r.AddAsync(It.Is<Domain.AssignmentToUser>(p => ValidateEquality(p)))).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IAssignmentToUserCreated>(It.Is<IAssignmentToUserCreated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var meansVerifier = new Mock<IAuthenticationMeansVerifier>(MockBehavior.Strict);
            meansVerifier.Setup(v => v.AssertExists(MeansId)).Returns(Task.CompletedTask).Verifiable();

            var userVerifier = new Mock<IUserVerifier>(MockBehavior.Strict);
            userVerifier.Setup(v => v.AssertExists(UserId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateAssignmentToUserHandler(repository.Object, busPublisher.Object, meansVerifier.Object, userVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
            meansVerifier.Verify();
            userVerifier.Verify();
        }
    }
}
