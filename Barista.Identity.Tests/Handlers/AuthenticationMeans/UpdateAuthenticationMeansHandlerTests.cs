using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.AssignmentToUser;
using Barista.Contracts.Commands.AuthenticationMeans;
using Barista.Contracts.Events.AssignmentToUser;
using Barista.Contracts.Events.AuthenticationMeans;
using Barista.Identity.Handlers.AssignmentToUser;
using Barista.Identity.Handlers.AuthenticationMeans;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Identity.Tests.Handlers.AuthenticationMeans
{
    [TestClass]
    public class UpdateAuthenticationMeansHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private const string Type = "rfid";
        private static readonly DateTimeOffset ValidSince = new DateTimeOffset(2010, 1, 1, 1, 1, 1, TimeSpan.Zero);
        private static readonly DateTimeOffset ValidUntil = new DateTimeOffset(2020, 1, 1, 1, 1, 1, TimeSpan.Zero);
        
        private readonly Mock<IUpdateAuthenticationMeans> _command;
        private IUpdateAuthenticationMeans Cmd => _command.Object;

        public UpdateAuthenticationMeansHandlerTests()
        {
            _command = new Mock<IUpdateAuthenticationMeans>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.Type).Returns(Type);
            _command.SetupGet(c => c.ValidSince).Returns(ValidSince);
            _command.SetupGet(c => c.ValidUntil).Returns(ValidUntil);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.Type, domainObjectOrEvent.Type);
            Assert.AreEqual(Cmd.ValidSince, domainObjectOrEvent.ValidSince);
            Assert.AreEqual(Cmd.ValidUntil, domainObjectOrEvent.ValidUntil);
            return true;
        }

        [TestMethod]
        public void UpdatesMeansInRepository_RaisesIntegrationEvent()
        {
            var authenticationMeans = new Domain.AuthenticationMeans(Id, "xxx", "Abcd", DateTimeOffset.UtcNow, DateTimeOffset.MaxValue);
            var repository = new Mock<IAuthenticationMeansRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(Id)).Returns(Task.FromResult(authenticationMeans)).Verifiable();
            repository.Setup(r => r.UpdateAsync(authenticationMeans)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IAuthenticationMeansUpdated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new UpdateAuthenticationMeansHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            ValidateEquality(authenticationMeans);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
