using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.Product;
using Barista.Contracts.Events.Product;
using Barista.Products.Handlers.Product;
using Barista.Products.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Products.Tests.Handlers.Products
{
    [TestClass]
    public class DeleteProductHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");

        private readonly Mock<IDeleteProduct> _command;
        private IDeleteProduct Cmd => _command.Object;

        public DeleteProductHandlerTests()
        {
            _command = new Mock<IDeleteProduct>();
            _command.SetupGet(c => c.Id).Returns(Id);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            return true;
        }

        [TestMethod]
        public void AddsProductToRepository_RaisesIntegrationEvent()
        {
            var product = new Domain.Product(Id, "Product Name", null);
            var repository = new Mock<IProductRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(product.Id)).Returns(Task.FromResult(product)).Verifiable();
            repository.Setup(r => r.DeleteAsync(product)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IProductDeleted>(It.Is<IProductDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteProductHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
