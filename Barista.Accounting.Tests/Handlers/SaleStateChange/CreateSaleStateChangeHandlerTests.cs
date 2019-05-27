using System;
using System.Threading.Tasks;
using Barista.Accounting.Domain;
using Barista.Accounting.Handlers.SaleStateChange;
using Barista.Accounting.Repositories;
using Barista.Accounting.Verifiers;
using Barista.Common;
using Barista.Contracts.Commands.SaleStateChange;
using Barista.Contracts.Events.SaleStateChange;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Accounting.Tests.Handlers.SaleStateChange
{
    [TestClass]
    public class CreateSaleStateChangeHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC6");
        private static readonly Guid ParentSaleId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");
        private static readonly Guid CausedByPointOfSaleId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DCA");
        private static readonly Guid CausedByUserId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC8");
        private const string Reason = "Why";
        private static readonly string State = SaleState.FundsReserved.ToString();

        private readonly Mock<ICreateSaleStateChange> _command;
        private ICreateSaleStateChange Cmd => _command.Object;

        public CreateSaleStateChangeHandlerTests()
        {
            _command = new Mock<ICreateSaleStateChange>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.ParentSaleId).Returns(ParentSaleId);
            _command.SetupGet(c => c.CausedByPointOfSaleId).Returns(CausedByPointOfSaleId);
            _command.SetupGet(c => c.CausedByUserId).Returns(CausedByUserId);
            _command.SetupGet(c => c.Reason).Returns(Reason);
            _command.SetupGet(c => c.State).Returns(State);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.CausedByPointOfSaleId, domainObjectOrEvent.CausedByPointOfSaleId);
            Assert.AreEqual(Cmd.CausedByUserId, domainObjectOrEvent.CausedByUserId);
            Assert.AreEqual(Cmd.Reason, domainObjectOrEvent.Reason);
            Assert.AreEqual(Cmd.State, domainObjectOrEvent.State.ToString());
            return true;
        }

        [TestMethod]
        public void AddsSaleStateChangeToRepository_RaisesIntegrationEvent()
        {
            var sale = new Accounting.Domain.Sale(Cmd.ParentSaleId, 9, 9, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);

            var repository = new Mock<ISalesRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(Cmd.ParentSaleId)).Returns(Task.FromResult(sale));
            repository.Setup(r => r.UpdateAsync(sale)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var publisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            publisher.Setup(p => p.Publish(It.Is<ISaleStateChangeCreated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var userVerifier = new Mock<IUserVerifier>(MockBehavior.Strict);
            userVerifier.Setup(v => v.AssertExists(CausedByUserId)).Returns(Task.CompletedTask).Verifiable();

            var posVerifier = new Mock<IPointOfSaleVerifier>(MockBehavior.Strict);
            posVerifier.Setup(v => v.AssertExists(CausedByPointOfSaleId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateSaleStateChangeHandler(repository.Object, publisher.Object, userVerifier.Object, posVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();
            Assert.IsTrue(result.Successful);

            ValidateEquality(sale.MostRecentStateChange);
            repository.Verify();
            publisher.Verify();
            userVerifier.Verify();
            posVerifier.Verify();
        }
    }
}
