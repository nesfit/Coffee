using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Contracts.Commands.SaleBasedStockOperation;
using Barista.Contracts.Events.SaleBasedStockOperation;
using Barista.StockOperations.Handlers.SaleBasedStockOperation;
using Barista.StockOperations.Repositories;
using Barista.StockOperations.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.StockOperations.Tests.Handlers.SaleBasedStockOperation
{
    [TestClass]
    public class UpdateSaleBasedStockOperationHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private static readonly Guid SaleId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4776");
        private const decimal Quantity = 10;
        private static readonly Guid StockItemId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4777");

        private readonly Mock<IUpdateSaleBasedStockOperation> _command;
        private IUpdateSaleBasedStockOperation Cmd => _command.Object;

        public UpdateSaleBasedStockOperationHandlerTests()
        {
            _command = new Mock<IUpdateSaleBasedStockOperation>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.SaleId).Returns(SaleId);
            _command.SetupGet(c => c.Quantity).Returns(Quantity);
            _command.SetupGet(c => c.StockItemId).Returns(StockItemId);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.SaleId, domainObjectOrEvent.SaleId);
            Assert.AreEqual(Cmd.Quantity, domainObjectOrEvent.Quantity);
            Assert.AreEqual(Cmd.StockItemId, domainObjectOrEvent.StockItemId);
            return true;
        }

        [TestMethod]
        public void UpdatesStockOperationInRepository_RaisesIntegrationEvent()
        {
            var stockOperation = new Domain.SaleBasedStockOperation(Id, Guid.Empty, 1, Guid.Empty);
            
            var repository = new Mock<IStockOperationRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(stockOperation.Id)).Returns(Task.FromResult<Domain.StockOperation>(stockOperation)).Verifiable();
            repository.Setup(r => r.UpdateAsync(stockOperation)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<ISaleBasedStockOperationUpdated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var siVerifier = new Mock<IStockItemVerifier>(MockBehavior.Strict);
            siVerifier.Setup(v => v.AssertExists(StockItemId)).Returns(Task.CompletedTask).Verifiable();

            var saleVerifier = new Mock<ISaleVerifier>(MockBehavior.Strict);
            saleVerifier.Setup(v => v.AssertExists(SaleId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new UpdateSaleBasedStockOperationHandler(repository.Object, busPublisher.Object, siVerifier.Object, saleVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
            siVerifier.Verify();
            saleVerifier.Verify();
            ValidateEquality(stockOperation);
        }
    }
}
