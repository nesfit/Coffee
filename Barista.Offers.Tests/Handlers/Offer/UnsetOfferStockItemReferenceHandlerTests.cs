using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts.Commands.Offer;
using Barista.Contracts.Events.Offer;
using Barista.Offers.Handlers.Offer;
using Barista.Offers.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Offers.Tests.Handlers.Offer
{
    [TestClass]
    public class UnsetOfferStockItemReferenceHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private static readonly Guid PointOfSaleId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4776");
        private static readonly Guid ProductId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4777");
        private static readonly Guid StockItemId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4778");
        private static readonly DateTimeOffset ValidSince = TestDateTimes.Year2001;
        private static readonly DateTimeOffset ValidUntil = TestDateTimes.Year2005;
        private const decimal RecommendedPrice = 42;

        private readonly Mock<IUnsetOfferStockItemReference> _command;
        private IUnsetOfferStockItemReference Cmd => _command.Object;

        public UnsetOfferStockItemReferenceHandlerTests()
        {
            _command = new Mock<IUnsetOfferStockItemReference>(MockBehavior.Strict);
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.StockItemIdToUnset).Returns(StockItemId);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(PointOfSaleId, domainObjectOrEvent.PointOfSaleId);
            Assert.AreEqual(ProductId, domainObjectOrEvent.ProductId);
            Assert.AreEqual(RecommendedPrice, domainObjectOrEvent.RecommendedPrice);
            Assert.AreEqual(null, domainObjectOrEvent.StockItemId);
            Assert.AreEqual(ValidSince, domainObjectOrEvent.ValidSince);
            Assert.AreEqual(ValidUntil, domainObjectOrEvent.ValidUntil);
            return true;
        }

        [TestMethod]
        public void UpdatesOfferInRepository_RaisesIntegrationEvent()
        {
            var offer = new Domain.Offer(Id, PointOfSaleId, ProductId, RecommendedPrice, StockItemId, ValidSince, ValidUntil);
            var repository = new Mock<IOfferRepository>(MockBehavior.Strict);
            
            repository.Setup(r => r.GetAsync(offer.Id)).Returns(Task.FromResult(offer)).Verifiable();
            repository.Setup(r => r.UpdateAsync(offer)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IOfferUpdated>(It.Is<IOfferUpdated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new UnsetOfferStockItemReferenceHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            ValidateEquality(offer);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
