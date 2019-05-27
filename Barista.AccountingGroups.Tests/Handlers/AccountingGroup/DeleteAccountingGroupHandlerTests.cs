using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Handlers.AccountingGroup;
using Barista.AccountingGroups.Repositories;
using Barista.Common;
using Barista.Contracts.Commands.AccountingGroup;
using Barista.Contracts.Events.AccountingGroup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.AccountingGroups.Tests.Handlers.AccountingGroup
{
    [TestClass]
    public class DeleteAccountingGroupHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");

        private readonly Mock<IDeleteAccountingGroup> _command;
        private IDeleteAccountingGroup Cmd => _command.Object;

        public DeleteAccountingGroupHandlerTests()
        {
            _command = new Mock<IDeleteAccountingGroup>();
            _command.SetupGet(c => c.Id).Returns(Id);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            return true;
        }

        [TestMethod]
        public void DeletesAccountingGroupFromRepository_RaisesIntegrationEvent()
        {
            var accountingGroup = new AccountingGroups.Domain.AccountingGroup(Id, "DisplayName", Guid.Empty);

            var repository = new Mock<IAccountingGroupRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(Id)).Returns(Task.FromResult(accountingGroup)).Verifiable();
            repository.Setup(r => r.DeleteAsync(accountingGroup)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IAccountingGroupDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteAccountingGroupHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
