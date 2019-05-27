using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Domain;
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
    public class DeleteUserAuthorizationHandlerTests
    {
        private static readonly Guid UserId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");
        private static readonly Guid AccountingGroupId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");

        private readonly Mock<IDeleteAccountingGroupUserAuthorization> _command;
        private IDeleteAccountingGroupUserAuthorization Cmd => _command.Object;

        public DeleteUserAuthorizationHandlerTests()
        {
            _command = new Mock<IDeleteAccountingGroupUserAuthorization>();
            _command.SetupGet(c => c.UserId).Returns(UserId);
            _command.SetupGet(c => c.AccountingGroupId).Returns(AccountingGroupId);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.UserId, domainObjectOrEvent.UserId);
            Assert.AreEqual(Cmd.AccountingGroupId, domainObjectOrEvent.AccountingGroupId);
            return true;
        }

        [TestMethod]
        public void DeletesUserAuthorizationFromRepository_RaisesIntegrationEvent()
        {
            var userAuth = new AccountingGroups.Domain.UserAuthorization(AccountingGroupId, UserId, UserAuthorizationLevel.Owner);

            var repository = new Mock<IUserAuthorizationRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(AccountingGroupId, UserId)).Returns(Task.FromResult(userAuth)).Verifiable();
            repository.Setup(r => r.DeleteAsync(userAuth)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IAccountingGroupUserAuthorizationDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteUserAuthorizationHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
