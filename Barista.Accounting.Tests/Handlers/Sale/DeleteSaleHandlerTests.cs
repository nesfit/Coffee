using System;
using System.Threading.Tasks;
using Barista.Accounting.Handlers.Sale;
using Barista.Accounting.Repositories;
using Barista.Common;
using Barista.Contracts.Commands.Sale;
using Barista.Contracts.Events.Sale;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Accounting.Tests.Handlers.Sale
{
    [TestClass]
    public class DeleteSaleHandlerTests
    {
        private static readonly Guid SaleGuid = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");

        private readonly Mock<IDeleteSale> _command;
        private IDeleteSale Cmd => _command.Object;
        
        public DeleteSaleHandlerTests()
        {
            _command = new Mock<IDeleteSale>();
            _command.SetupGet(c => c.Id).Returns(SaleGuid);
        }

        private bool IsExpectedEvent(ISaleDeleted e)
        {
            Assert.AreEqual(Cmd.Id, e.Id);
            return true;
        }

        [TestMethod]
        public void DeletesSaleFromRepository_RaisesIntegrationEvent()
        {
            var sale = new Accounting.Domain.Sale(SaleGuid, 1, 2, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);

            var repository = new Mock<ISalesRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(sale.Id)).Returns(Task.FromResult(sale)).Verifiable();
            repository.Setup(r => r.DeleteAsync(sale)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<ISaleDeleted>(e => IsExpectedEvent(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteSaleHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();
            Assert.IsTrue(result.Successful);

            repository.Verify();
            busPublisher.Verify();
        }
    }
}
