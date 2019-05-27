using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Contracts.Commands.StockItem;
using Barista.Contracts.Events.StockItem;
using Barista.StockItems.Handlers.StockItem;
using Barista.StockItems.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.StockItems.Tests.Handlers.StockItem
{
    [TestClass]
    public class DeleteStockItemHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private static readonly Guid PosId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4776");

        private readonly Mock<IDeleteStockItem> _command;
        private IDeleteStockItem Cmd => _command.Object;

        public DeleteStockItemHandlerTests()
        {
            _command = new Mock<IDeleteStockItem>();
            _command.SetupGet(c => c.Id).Returns(Id);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            return true;
        }

        [TestMethod]
        public void DeletesStockItemFromRepository_RaisesIntegrationEvent()
        {
            var stockItem = new Domain.StockItem(Id, "Display Name", PosId);
            
            var repository = new Mock<IStockItemRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(stockItem.Id)).Returns(Task.FromResult(stockItem)).Verifiable();
            repository.Setup(r => r.DeleteAsync(stockItem)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IStockItemDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteStockItemHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
            ValidateEquality(stockItem);
        }
    }
}
