using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.PointOfSale;
using Barista.Contracts.Events.PointOfSale;
using Barista.PointsOfSale.Handlers.PointOfSale;
using Barista.PointsOfSale.Repositories;
using Barista.PointsOfSale.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.PointsOfSale.Tests.Handlers.PointOfSale
{
    [TestClass]
    public class CreatePointOfSaleHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");
        private static readonly Guid SaleStrategyId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DB7");
        private static readonly Guid ParentAccountingGroupId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DA7");
        private const string DisplayName = "PoS";

        private readonly Mock<ICreatePointOfSale> _command;
        private ICreatePointOfSale Cmd => _command.Object;

        public CreatePointOfSaleHandlerTests()
        {
            _command = new Mock<ICreatePointOfSale>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.DisplayName).Returns(DisplayName);
            _command.SetupGet(c => c.SaleStrategyId).Returns(SaleStrategyId);
            _command.SetupGet(c => c.ParentAccountingGroupId).Returns(ParentAccountingGroupId);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.DisplayName, domainObjectOrEvent.DisplayName);
            Assert.AreEqual(Cmd.SaleStrategyId, domainObjectOrEvent.SaleStrategyId);
            return true;
        }

        [TestMethod]
        public void AddsPointOfSaleToRepository_RaisesIntegrationEvent()
        {
            var repository = new Mock<IPointOfSaleRepository>(MockBehavior.Strict);
            repository.Setup(r => r.AddAsync(It.Is<Domain.PointOfSale>(p => ValidateEquality(p)))).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IPointOfSaleCreated>(It.Is<IPointOfSaleCreated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var agVerifier = new Mock<IAccountingGroupVerifier>(MockBehavior.Strict);
            agVerifier.Setup(v => v.AssertExists(ParentAccountingGroupId)).Returns(Task.CompletedTask).Verifiable();

            var ssVerifier = new Mock<ISaleStrategyVerifier>(MockBehavior.Strict);
            ssVerifier.Setup(v => v.AssertExists(SaleStrategyId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreatePointOfSaleHandler(repository.Object, busPublisher.Object, agVerifier.Object, ssVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
            agVerifier.Verify();
            ssVerifier.Verify();
        }
    }
}
