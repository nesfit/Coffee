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
    public class UpdateAuthenticationMeansValueHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private const string Type = "rfid";
        private static readonly DateTimeOffset ValidSince = new DateTimeOffset(2010, 1, 1, 1, 1, 1, TimeSpan.Zero);
        private static readonly DateTimeOffset ValidUntil = new DateTimeOffset(2020, 1, 1, 1, 1, 1, TimeSpan.Zero);
        private static readonly string Value = "NewValue";
        
        private readonly Mock<IUpdateAuthenticationMeansValue> _command;
        private IUpdateAuthenticationMeansValue Cmd => _command.Object;

        public UpdateAuthenticationMeansValueHandlerTests()
        {
            _command = new Mock<IUpdateAuthenticationMeansValue>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.Value).Returns(Value);
        }

        private bool ValidateEquality(Domain.AuthenticationMeans domainObject)
        {
            Assert.AreEqual(Cmd.Id, domainObject.Id);
            Assert.AreEqual(Cmd.Value, domainObject.Value);
            return true;
        }

        private bool ValidateEquality(IAuthenticationMeansUpdated @event)
        {
            Assert.AreEqual(Cmd.Id, @event.Id);
            Assert.AreEqual(Type, @event.Type);
            Assert.AreEqual(ValidSince, @event.ValidSince);
            Assert.AreEqual(ValidUntil, @event.ValidUntil);
            return true;
        }

        [TestMethod]
        public void UpdatesMeansValueInRepository_RaisesIntegrationEvent()
        {
            var authenticationMeans = new Domain.AuthenticationMeans(Id, Type, "OldValue", ValidSince, ValidUntil);
            var repository = new Mock<IAuthenticationMeansRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(Id)).Returns(Task.FromResult(authenticationMeans)).Verifiable();
            repository.Setup(r => r.UpdateAsync(authenticationMeans)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IAuthenticationMeansUpdated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new UpdateAuthenticationMeansValueHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            ValidateEquality(authenticationMeans);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
