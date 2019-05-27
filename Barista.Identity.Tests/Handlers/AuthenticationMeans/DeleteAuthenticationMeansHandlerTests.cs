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
    public class DeleteAuthenticationMeansHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");

        private readonly Mock<IDeleteAuthenticationMeans> _command;
        private IDeleteAuthenticationMeans Cmd => _command.Object;

        public DeleteAuthenticationMeansHandlerTests()
        {
            _command = new Mock<IDeleteAuthenticationMeans>();
            _command.SetupGet(c => c.Id).Returns(Id);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            return true;
        }

        [TestMethod]
        public void DeletesMeansFromRepository_RaisesIntegrationEvent()
        {
            var authenticationMeans = new Domain.AuthenticationMeans(Id, "xxx", "Abcd", DateTimeOffset.UtcNow, DateTimeOffset.MaxValue);

            var repository = new Mock<IAuthenticationMeansRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(Id)).Returns(Task.FromResult(authenticationMeans)).Verifiable();
            repository.Setup(r => r.DeleteAsync(authenticationMeans)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IAuthenticationMeansDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteAuthenticationMeansHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
