using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Contracts.Commands.AuthenticationMeans;
using Barista.Contracts.Events.AuthenticationMeans;
using Barista.Identity.Handlers.AuthenticationMeans;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Identity.Tests.Handlers.AuthenticationMeans
{
    [TestClass]
    public class CreateAuthenticationMeansHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private const string Type = "rfid";
        private static readonly DateTimeOffset ValidSince = new DateTimeOffset(2010, 1, 1, 1, 1, 1, TimeSpan.Zero);
        private static readonly DateTimeOffset ValidUntil = new DateTimeOffset(2020, 1, 1, 1, 1, 1, TimeSpan.Zero);
        private static readonly string Value = "value";

        private readonly Mock<ICreateAuthenticationMeans> _command;
        private ICreateAuthenticationMeans Cmd => _command.Object;

        public CreateAuthenticationMeansHandlerTests()
        {
            _command = new Mock<ICreateAuthenticationMeans>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.Method).Returns(Type);
            _command.SetupGet(c => c.ValidSince).Returns(ValidSince);
            _command.SetupGet(c => c.ValidUntil).Returns(ValidUntil);
            _command.SetupGet(c => c.Value).Returns(Value);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.Method, domainObjectOrEvent.Type);
            Assert.AreEqual(Cmd.ValidSince, domainObjectOrEvent.ValidSince);
            Assert.AreEqual(Cmd.ValidUntil, domainObjectOrEvent.ValidUntil);
            return true;
        }

        [TestMethod]
        public void AddsMeansToRepository_RaisesIntegrationEvent()
        {
            var repository = new Mock<IAuthenticationMeansRepository>(MockBehavior.Strict);
            repository.Setup(r => r.AddAsync(It.Is<Domain.AuthenticationMeans>(p => ValidateEquality(p)))).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IAuthenticationMeansCreated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateAuthenticationMeansHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
