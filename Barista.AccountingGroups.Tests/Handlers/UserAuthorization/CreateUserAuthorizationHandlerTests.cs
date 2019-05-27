using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Handlers.UserAuthorization;
using Barista.AccountingGroups.Repositories;
using Barista.AccountingGroups.Verifiers;
using Barista.Common;
using Barista.Contracts.Commands.AccountingGroupUserAuthorization;
using Barista.Contracts.Events.AccountingGroupUserAuthorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.AccountingGroups.Tests.Handlers.UserAuthorization
{
    [TestClass]
    public class CreateUserAuthorizationHandlerTests
    {
        private static readonly Guid AccountingGroupId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DB7");
        private static readonly Guid UserId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DA7");
        private const string Level = "Owner";

        private readonly Mock<ICreateAccountingGroupUserAuthorization> _command;
        private ICreateAccountingGroupUserAuthorization Cmd => _command.Object;

        public CreateUserAuthorizationHandlerTests()
        {
            _command = new Mock<ICreateAccountingGroupUserAuthorization>();
            _command.SetupGet(c => c.AccountingGroupId).Returns(AccountingGroupId);
            _command.SetupGet(c => c.UserId).Returns(UserId);
            _command.SetupGet(c => c.Level).Returns(Level);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.AccountingGroupId, domainObjectOrEvent.AccountingGroupId);
            Assert.AreEqual(Cmd.UserId, domainObjectOrEvent.UserId);
            Assert.AreEqual(Cmd.Level, domainObjectOrEvent.Level.ToString());
            return true;
        }

        [TestMethod]
        public void AddsAuthorizationToRepository_RaisesIntegrationEvent()
        {
            var repository = new Mock<IUserAuthorizationRepository>(MockBehavior.Strict);
            repository.Setup(r => r.AddAsync(It.Is<AccountingGroups.Domain.UserAuthorization>(p => ValidateEquality(p)))).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IAccountingGroupUserAuthorizationCreated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var userVerifier = new Mock<IUserVerifier>(MockBehavior.Strict);
            userVerifier.Setup(v => v.AssertExists(UserId)).Returns(Task.CompletedTask).Verifiable();
            
            var agVerifier = new Mock<IAccountingGroupVerifier>(MockBehavior.Strict);
            agVerifier.Setup(v => v.AssertExists(AccountingGroupId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateUserAuthorizationHandler(repository.Object, busPublisher.Object, agVerifier.Object, userVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
            agVerifier.Verify();
            userVerifier.Verify();
        }
    }
}
