using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.ManualStockOperation;
using Barista.Contracts.Events.ManualStockOperation;
using Barista.StockOperations.Domain;
using Barista.StockOperations.Handlers.ManualStockOperation;
using Barista.StockOperations.Repositories;
using Barista.StockOperations.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.StockOperations.Tests.Handlers.ManualStockOperation
{
    [TestClass]
    public class UpdateManualStockOperationHandlerTests
    {
        private static readonly Guid Id = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4775");
        private const string Comment = "Stock operation comment";
        private static readonly Guid CreatedByUserId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4776");
        private const decimal Quantity = 10;
        private static readonly Guid StockItemId = Guid.Parse("B5FA1B9A-95A7-4083-9B1A-03E8D92D4777");

        private readonly Mock<IUpdateManualStockOperation> _command;
        private IUpdateManualStockOperation Cmd => _command.Object;

        public UpdateManualStockOperationHandlerTests()
        {
            _command = new Mock<IUpdateManualStockOperation>();
            _command.SetupGet(c => c.Id).Returns(Id);
            _command.SetupGet(c => c.Comment).Returns(Comment);
            _command.SetupGet(c => c.CreatedByUserId).Returns(CreatedByUserId);
            _command.SetupGet(c => c.Quantity).Returns(Quantity);
            _command.SetupGet(c => c.StockItemId).Returns(StockItemId);
        }

        private bool ValidateEquality(dynamic domainObjectOrEvent)
        {
            Assert.AreEqual(Cmd.Id, domainObjectOrEvent.Id);
            Assert.AreEqual(Cmd.Comment, domainObjectOrEvent.Comment);
            Assert.AreEqual(Cmd.CreatedByUserId, domainObjectOrEvent.CreatedByUserId);
            Assert.AreEqual(Cmd.Quantity, domainObjectOrEvent.Quantity);
            Assert.AreEqual(Cmd.StockItemId, domainObjectOrEvent.StockItemId);
            return true;
        }

        [TestMethod]
        public void UpdatesStockOperationInRepository_RaisesIntegrationEvent()
        {
            var stockOperation = new Domain.ManualStockOperation(Id, Guid.Empty, 1, Guid.Empty, string.Empty);
            
            var repository = new Mock<IStockOperationRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(stockOperation.Id)).Returns(Task.FromResult<Domain.StockOperation>(stockOperation)).Verifiable();
            repository.Setup(r => r.UpdateAsync(stockOperation)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish<IManualStockOperationUpdated>(It.Is<IManualStockOperationUpdated>(e => ValidateEquality(e)))).Returns(Task.CompletedTask).Verifiable();

            var siVerifier = new Mock<IStockItemVerifier>(MockBehavior.Strict);
            siVerifier.Setup(v => v.AssertExists(StockItemId)).Returns(Task.CompletedTask).Verifiable();

            var userVerifier = new Mock<IUserVerifier>(MockBehavior.Strict);
            userVerifier.Setup(v => v.AssertExists(CreatedByUserId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new UpdateManualStockOperationHandler(repository.Object, busPublisher.Object, siVerifier.Object, userVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            repository.Verify();
            busPublisher.Verify();
            siVerifier.Verify();
            userVerifier.Verify();
            ValidateEquality(stockOperation);
        }
    }
}
