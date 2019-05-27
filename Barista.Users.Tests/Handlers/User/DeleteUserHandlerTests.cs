using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.User;
using Barista.Contracts.Events.User;
using Barista.Users.Handlers.User;
using Barista.Users.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Users.Tests.Handlers.User
{
    [TestClass]
    public class DeleteUserHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");

        private readonly Mock<IDeleteUser> _command;
        private IDeleteUser Cmd => _command.Object;

        public DeleteUserHandlerTests()
        {
            _command = new Mock<IDeleteUser>();
            _command.SetupGet(c => c.Id).Returns(Id);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            return true;
        }

        [TestMethod]
        public void UpdatesUserInRepository_RaisesIntegrationEvent()
        {
            var user = new Domain.User(Id, "A B", "a@b.cz", false);

            var repository = new Mock<IUserRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(user.Id)).Returns(Task.FromResult(user)).Verifiable();
            repository.Setup(r => r.DeleteAsync(user)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IUserDeleted>(It.Is<IUserDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteUserHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
