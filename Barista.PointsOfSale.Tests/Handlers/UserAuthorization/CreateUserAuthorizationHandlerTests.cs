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
    public class CreateUserAuthorizationHandlerTests
    {
        private static readonly Guid PointOfSaleId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");
        private static readonly Guid UserId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DB7");
        private static readonly string Level = UserAuthorizationLevel.AuthorizedUser.ToString();

        private readonly Mock<ICreatePointOfSaleUserAuthorization> _command;
        private ICreatePointOfSaleUserAuthorization Cmd => _command.Object;

        public CreateUserAuthorizationHandlerTests()
        {
            _command = new Mock<ICreatePointOfSaleUserAuthorization>();
            _command.SetupGet(c => c.PointOfSaleId).Returns(PointOfSaleId);
            _command.SetupGet(c => c.UserId).Returns(UserId);
            _command.SetupGet(c => c.Level).Returns(Level);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.PointOfSaleId, domainObjectOrEvent.PointOfSaleId);
            Assert.AreEqual(Cmd.UserId, domainObjectOrEvent.UserId);
            Assert.AreEqual(Cmd.Level, domainObjectOrEvent.Level.ToString());
            return true;
        }

        [TestMethod]
        public void AddsAuthorizationToRepository_RaisesIntegrationEvent()
        {
            var repository = new Mock<IUserAuthorizationRepository>(MockBehavior.Strict);
            repository.Setup(r => r.AddAsync(It.Is<Domain.UserAuthorization>(p => ValidateEquality(p)))).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IPointOfSaleUserAuthorizationCreated>(It.Is<IPointOfSaleUserAuthorizationCreated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateUserAuthorizationHandler(repository.Object, busPublisher.Object);
            handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            repository.Verify();
            busPublisher.Verify();
        }
    }
}
