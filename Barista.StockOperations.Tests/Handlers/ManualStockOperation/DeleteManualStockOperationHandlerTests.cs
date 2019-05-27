using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Contracts.Commands.ManualStockOperation;
using Barista.Contracts.Events.ManualStockOperation;
using Barista.StockOperations.Handlers.ManualStockOperation;
using Barista.StockOperations.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.StockOperations.Tests.Handlers.ManualStockOperation
{
    [TestClass]
    public class DeleteManualStockOperationHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");

        private readonly Mock<IDeleteManualStockOperation> _command;
        private IDeleteManualStockOperation Cmd => _command.Object;

        public DeleteManualStockOperationHandlerTests()
        {
            _command = new Mock<IDeleteManualStockOperation>();
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
            var stockOperation = new Domain.ManualStockOperation(Id, Guid.Empty, 1, Guid.Empty, string.Empty);
            
            var repository = new Mock<IStockOperationRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(stockOperation.Id)).Returns(Task.FromResult<Domain.StockOperation>(stockOperation)).Verifiable();
            repository.Setup(r => r.DeleteAsync(stockOperation)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IManualStockOperationDeleted>(It.Is<IManualStockOperationDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteManualStockOperationHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
