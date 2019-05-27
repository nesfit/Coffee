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
    public class CreateUserHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private const string FullName = "Firstname Lastname";
        private const string EmailAddress = "someone@example.com";
        private const bool IsAdministrator = true;

        private readonly Mock<ICreateUser> _command;
        private ICreateUser Cmd => _command.Object;

        public CreateUserHandlerTests()
        {
            _command = new Mock<ICreateUser>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.FullName).Returns(FullName);
            _command.SetupGet(c => c.EmailAddress).Returns(EmailAddress);
            _command.SetupGet(c => c.IsAdministrator).Returns(IsAdministrator);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.FullName, domainObjectOrEvent.FullName);
            Assert.AreEqual(Cmd.EmailAddress, domainObjectOrEvent.EmailAddress);
            Assert.AreEqual(Cmd.IsAdministrator, domainObjectOrEvent.IsAdministrator);
            return true;
        }

        [TestMethod]
        public void AddsUserToRepository_RaisesIntegrationEvent()
        {
            var repository = new Mock<IUserRepository>(MockBehavior.Strict);
            repository.Setup(r => r.AddAsync(It.Is<Domain.User>(p => ValidateEquality(p)))).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IUserCreated>(It.Is<IUserCreated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateUserHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
