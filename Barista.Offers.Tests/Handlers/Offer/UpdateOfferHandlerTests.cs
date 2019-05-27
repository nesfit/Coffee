using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.AuthenticationMeans;
using Barista.Contracts.Commands.Offer;
using Barista.Contracts.Events.AuthenticationMeans;
using Barista.Contracts.Events.Offer;
using Barista.Offers.Handlers.Offer;
using Barista.Offers.Repositories;
using Barista.Offers.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Offers.Tests.Handlers.Offer
{
    [TestClass]
    public class UpdateOfferHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private static readonly Guid PointOfSaleId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4776");
        private static readonly Guid ProductId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4777");
        private static readonly Guid StockItemId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4778");
        private static readonly DateTimeOffset ValidSince = new DateTimeOffset(2010, 1, 1, 1, 1, 1, TimeSpan.Zero);
        private static readonly DateTimeOffset ValidUntil = new DateTimeOffset(2020, 1, 1, 1, 1, 1, TimeSpan.Zero);
        private const decimal RecommendedPrice = 42;

        private readonly Mock<IUpdateOffer> _command;
        private IUpdateOffer Cmd => _command.Object;

        public UpdateOfferHandlerTests()
        {
            _command = new Mock<IUpdateOffer>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.PointOfSaleId).Returns(PointOfSaleId);
            _command.SetupGet(c => c.ProductId).Returns(ProductId);
            _command.SetupGet(c => c.RecommendedPrice).Returns(RecommendedPrice);
            _command.SetupGet(c => c.StockItemId).Returns(StockItemId);
            _command.SetupGet(c => c.ValidSince).Returns(ValidSince);
            _command.SetupGet(c => c.ValidUntil).Returns(ValidUntil);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.PointOfSaleId, domainObjectOrEvent.PointOfSaleId);
            Assert.AreEqual(Cmd.ProductId, domainObjectOrEvent.ProductId);
            Assert.AreEqual(Cmd.RecommendedPrice, domainObjectOrEvent.RecommendedPrice);
            Assert.AreEqual(Cmd.StockItemId, domainObjectOrEvent.StockItemId);
            Assert.AreEqual(Cmd.ValidSince, domainObjectOrEvent.ValidSince);
            Assert.AreEqual(Cmd.ValidUntil, domainObjectOrEvent.ValidUntil);
            return true;
        }

        [TestMethod]
        public void UpdatesOfferInRepository_RaisesIntegrationEvent()
        {
            var offer = new Domain.Offer(Id,Guid.Empty, Guid.Empty, null, null, null, null);
            var repository = new Mock<IOfferRepository>(MockBehavior.Strict);
            
            repository.Setup(r => r.GetAsync(offer.Id)).Returns(Task.FromResult(offer)).Verifiable();
            repository.Setup(r => r.UpdateAsync(offer)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IOfferUpdated>(It.Is<IOfferUpdated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var posVerifier = new Mock<IPointOfSaleVerifier>(MockBehavior.Strict);
            posVerifier.Setup(v => v.AssertExists(PointOfSaleId)).Returns(Task.CompletedTask).Verifiable();

            var productVerifier = new Mock<IProductVerifier>(MockBehavior.Strict);
            productVerifier.Setup(v => v.AssertExists(ProductId)).Returns(Task.CompletedTask).Verifiable();

            var stockItemVerifier = new Mock<IStockItemVerifier>(MockBehavior.Strict);
            stockItemVerifier.Setup(v => v.AssertExists(StockItemId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new UpdateOfferHandler(repository.Object, busPublisher.Object, posVerifier.Object, productVerifier.Object, stockItemVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            ValidateEquality(offer);
            repository.Verify();
            busPublisher.Verify();
            posVerifier.Verify();
            productVerifier.Verify();
            stockItemVerifier.Verify();
        }
    }
}
