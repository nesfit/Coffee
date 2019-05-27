using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Handlers.AccountingGroup;
using Barista.AccountingGroups.Repositories;
using Barista.AccountingGroups.Verifiers;
using Barista.Common;
using Barista.Contracts.Commands.AccountingGroup;
using Barista.Contracts.Events.AccountingGroup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.AccountingGroups.Tests.Handlers.AccountingGroup
{
    [TestClass]
    public class CreateAccountingGroupHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");
        private static readonly Guid SaleStrategyId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DB7");
        private const string DisplayName = "Src";

        private readonly Mock<ICreateAccountingGroup> _command;
        private ICreateAccountingGroup Cmd => _command.Object;

        public CreateAccountingGroupHandlerTests()
        {
            _command = new Mock<ICreateAccountingGroup>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.DisplayName).Returns(DisplayName);
            _command.SetupGet(c => c.SaleStrategyId).Returns(SaleStrategyId);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.DisplayName, domainObjectOrEvent.DisplayName);
            Assert.AreEqual(Cmd.SaleStrategyId, domainObjectOrEvent.SaleStrategyId);
            return true;
        }

        [TestMethod]
        public void AddsAccountingGroupToRepository_RaisesIntegrationEvent()
        {
            var repository = new Mock<IAccountingGroupRepository>(MockBehavior.Strict);
            repository.Setup(r => r.AddAsync(It.Is<AccountingGroups.Domain.AccountingGroup>(p => ValidateEquality(p)))).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IAccountingGroupCreated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var saleStrategyVerifier = new Mock<ISaleStrategyVerifier>(MockBehavior.Strict);
            saleStrategyVerifier.Setup(v => v.AssertExists(SaleStrategyId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateAccountingGroupHandler(repository.Object, busPublisher.Object, saleStrategyVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
            saleStrategyVerifier.Verify();
        }
    }
}
