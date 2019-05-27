using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.PointOfSale;
using Barista.Contracts.Events.PointOfSale;
using Barista.PointsOfSale.Handlers.PointOfSale;
using Barista.PointsOfSale.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.PointsOfSale.Tests.Handlers.PointOfSale
{
    [TestClass]
    public class DeletePointOfSaleHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");

        private readonly Mock<IDeletePointOfSale> _command;
        private IDeletePointOfSale Cmd => _command.Object;

        public DeletePointOfSaleHandlerTests()
        {
            _command = new Mock<IDeletePointOfSale>();
            _command.SetupGet(c => c.Id).Returns(Id);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            return true;
        }

        [TestMethod]
        public void DeletesPointOfSaleFromRepository_RaisesIntegrationEvent()
        {
            var pos = new Domain.PointOfSale(Id, "Old DN", Guid.Empty, Guid.Empty);

            var repository = new Mock<IPointOfSaleRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(pos.Id)).Returns(Task.FromResult(pos)).Verifiable();
            repository.Setup(r => r.DeleteAsync(pos)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IPointOfSaleDeleted>(It.Is<IPointOfSaleDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeletePointOfSaleHandler(repository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
        }
    }
}
