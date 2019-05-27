using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Contracts.Commands.SaleBasedStockOperation;
using Barista.Contracts.Events.SaleBasedStockOperation;
using Barista.StockOperations.Handlers.SaleBasedStockOperation;
using Barista.StockOperations.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.StockOperations.Tests.Handlers.SaleBasedStockOperation
{
    [TestClass]
    public class DeleteSaleBasedStockOperationHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");

        private readonly Mock<IDeleteSaleBasedStockOperation> _command;
        private IDeleteSaleBasedStockOperation Cmd => _command.Object;

        public DeleteSaleBasedStockOperationHandlerTests()
        {
            _command = new Mock<IDeleteSaleBasedStockOperation>();
            _command.SetupGet(c => c.Id).Returns(Id);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            return true;
        }

        [TestMethod]
        public void DeletesStockOperationFromRepository_RaisesIntegrationEvent()
        {
            var stockOperation = new Domain.SaleBasedStockOperation(Id, Guid.Empty, 1, Guid.Empty);
            
            var repository = new Mock<IStockOperationRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(stockOperation.Id)).Returns(Task.FromResult<Domain.StockOperation>(stockOperation)).Verifiable();
            repository.Setup(r => r.DeleteAsync(stockOperation)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<ISaleBasedStockOperationDeleted>(It.Is<ISaleBasedStockOperationDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteSaleBasedStockOperationHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
