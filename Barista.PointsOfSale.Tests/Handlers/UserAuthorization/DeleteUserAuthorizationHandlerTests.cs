using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.PointOfSale;
using Barista.Contracts.Commands.PointOfSaleUserAuthorization;
using Barista.Contracts.Events.PointOfSale;
using Barista.Contracts.Events.PointOfSaleUserAuthorization;
using Barista.PointsOfSale.Domain;
using Barista.PointsOfSale.Handlers.PointOfSale;
using Barista.PointsOfSale.Handlers.UserAuthorization;
using Barista.PointsOfSale.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.PointsOfSale.Tests.Handlers.UserAuthorization
{
    [TestClass]
    public class DeleteUserAuthorizationHandlerTests
    {
        private static readonly Guid PointOfSaleId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");
        private static readonly Guid UserId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DB7");

        private readonly Mock<IDeletePointOfSaleUserAuthorization> _command;
        private IDeletePointOfSaleUserAuthorization Cmd => _command.Object;

        public DeleteUserAuthorizationHandlerTests()
        {
            _command = new Mock<IDeletePointOfSaleUserAuthorization>();
            _command.SetupGet(c => c.PointOfSaleId).Returns(PointOfSaleId);
            _command.SetupGet(c => c.UserId).Returns(UserId);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.PointOfSaleId, domainObjectOrEvent.PointOfSaleId);
            Assert.AreEqual(Cmd.UserId, domainObjectOrEvent.UserId);
            return true;
        }

        [TestMethod]
        public void UpdatesAuthorizationInRepository_RaisesIntegrationEvent()
        {
            var ua = new Domain.UserAuthorization(PointOfSaleId, UserId, UserAuthorizationLevel.Owner);

            var repository = new Mock<IUserAuthorizationRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(PointOfSaleId, UserId)).Returns(Task.FromResult(ua)).Verifiable();
            repository.Setup(r => r.DeleteAsync(ua)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IPointOfSaleUserAuthorizationDeleted>(It.Is<IPointOfSaleUserAuthorizationDeleted>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteUserAuthorizationHandler(repository.Object, busPublisher.Object);
            handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            repository.Verify();
            busPublisher.Verify();
            ValidateEquality(ua);
        }
    }
}
