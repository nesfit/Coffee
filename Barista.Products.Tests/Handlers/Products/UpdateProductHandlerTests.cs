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
    public class UpdateProductHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private const string DisplayName = "Cup of Coffee";
        private const decimal RecommendedPrice = 5;

        private readonly Mock<IUpdateProduct> _command;
        private IUpdateProduct Cmd => _command.Object;

        public UpdateProductHandlerTests()
        {
            _command = new Mock<IUpdateProduct>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.DisplayName).Returns(DisplayName);
            _command.SetupGet(c => c.RecommendedPrice).Returns(RecommendedPrice);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.DisplayName, domainObjectOrEvent.DisplayName);
            Assert.AreEqual(Cmd.RecommendedPrice, domainObjectOrEvent.RecommendedPrice);
            return true;
        }

        [TestMethod]
        public void AddsProductToRepository_RaisesIntegrationEvent()
        {
            var product = new Domain.Product(Id, "Product Name", null);
            var repository = new Mock<IProductRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(product.Id)).Returns(Task.FromResult(product)).Verifiable();
            repository.Setup(r => r.UpdateAsync(product)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IProductUpdated>(It.Is<IProductUpdated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new UpdateProductHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
            ValidateEquality(product);
        }
    }
}
