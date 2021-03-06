﻿using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.StockItem;
using Barista.Contracts.Events.StockItem;
using Barista.StockItems.Handlers.StockItem;
using Barista.StockItems.Repositories;
using Barista.StockItems.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.StockItems.Tests.Handlers.StockItem
{
    [TestClass]
    public class CreateStockItemHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private static readonly Guid PosId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4776");
        private const string DisplayName = "StockItem";

        private readonly Mock<ICreateStockItem> _command;
        private ICreateStockItem Cmd => _command.Object;

        public CreateStockItemHandlerTests()
        {
            _command = new Mock<ICreateStockItem>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.PointOfSaleId).Returns(PosId);
            _command.SetupGet(c => c.DisplayName).Returns(DisplayName);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.PointOfSaleId, domainObjectOrEvent.PointOfSaleId);
            Assert.AreEqual(Cmd.DisplayName, domainObjectOrEvent.DisplayName);
            return true;
        }

        [TestMethod]
        public void AddsStockItemToRepository_RaisesIntegrationEvent()
        {
            var repository = new Mock<IStockItemRepository>(MockBehavior.Strict);
            repository.Setup(r => r.AddAsync(It.Is<Domain.StockItem>(p => ValidateEquality(p)))).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IStockItemCreated>(It.Is<IStockItemCreated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var posVerifier = new Mock<IPointOfSaleVerifier>(MockBehavior.Strict);
            posVerifier.Setup(v => v.AssertExists(PosId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateStockItemHandler(repository.Object, busPublisher.Object, posVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
            posVerifier.Verify();
        }
    }
}
