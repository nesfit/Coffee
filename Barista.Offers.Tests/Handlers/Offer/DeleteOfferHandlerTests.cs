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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Offers.Tests.Handlers.Offer
{
    [TestClass]
    public class DeleteOfferHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");

        private readonly Mock<IDeleteOffer> _command;
        private IDeleteOffer Cmd => _command.Object;

        public DeleteOfferHandlerTests()
        {
            _command = new Mock<IDeleteOffer>();
            _command.SetupGet(c => c.Id).Returns(Id);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            return true;
        }

        [TestMethod]
        public void DeletesOfferFromRepository_RaisesIntegrationEvent()
        {
            var offer = new Domain.Offer(Id,Guid.Empty, Guid.Empty, null, null, null, null);
            var repository = new Mock<IOfferRepository>(MockBehavior.Strict);
            
            repository.Setup(r => r.GetAsync(offer.Id)).Returns(Task.FromResult(offer)).Verifiable();
            repository.Setup(r => r.DeleteAsync(offer)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IOfferDeleted>(It.Is<IOfferDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteOfferHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
