using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Handlers.UserAuthorization;
using Barista.AccountingGroups.Repositories;
using Barista.Common;
using Barista.Contracts.Commands.AccountingGroupUserAuthorization;
using Barista.Contracts.Events.AccountingGroupUserAuthorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.AccountingGroups.Tests.Handlers.UserAuthorization
{
    [TestClass]
    public class UpdateUserAuthorizationHandlerTests
    {
        private static readonly Guid UserId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");
        private static readonly Guid AccountingGroupId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DB7");
        private static readonly string UserAuthorizationLevel = AccountingGroups.Domain.UserAuthorizationLevel.Owner.ToString();

        private readonly Mock<IUpdateAccountingGroupUserAuthorization> _command;
        private IUpdateAccountingGroupUserAuthorization Cmd => _command.Object;

        public UpdateUserAuthorizationHandlerTests()
        {
            _command = new Mock<IUpdateAccountingGroupUserAuthorization>();
            _command.SetupGet(c => c.UserId).Returns(UserId);
            _command.SetupGet(c => c.AccountingGroupId).Returns(AccountingGroupId);
            _command.SetupGet(c => c.Level).Returns(UserAuthorizationLevel);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.UserId, domainObjectOrEvent.UserId);
            Assert.AreEqual(Cmd.AccountingGroupId, domainObjectOrEvent.AccountingGroupId);
            Assert.AreEqual(Cmd.Level, domainObjectOrEvent.Level.ToString());
            return true;
        }

        [TestMethod]
        public void UpdatesUserAuthorizationInRepository_RaisesIntegrationEvent()
        {
            var userAuthorization = new AccountingGroups.Domain.UserAuthorization(AccountingGroupId, UserId, AccountingGroups.Domain.UserAuthorizationLevel.AuthorizedUser);

            var repository = new Mock<IUserAuthorizationRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(AccountingGroupId, UserId)).Returns(Task.FromResult(userAuthorization)).Verifiable();
            repository.Setup(r => r.UpdateAsync(userAuthorization)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IAccountingGroupUserAuthorizationUpdated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new UpdateUserAuthorizationHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            ValidateEquality(userAuthorization);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
