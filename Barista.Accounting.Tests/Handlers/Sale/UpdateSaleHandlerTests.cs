using System;
using System.Threading.Tasks;
using Barista.Accounting.Handlers.Sale;
using Barista.Accounting.Repositories;
using Barista.Accounting.Verifiers;
using Barista.Common;
using Barista.Contracts.Commands.Sale;
using Barista.Contracts.Events.Sale;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Accounting.Tests.Handlers.Sale
{
    [TestClass]
    public class UpdateSaleHandlerTests
    {
        private static readonly Guid SaleId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");
        private static readonly Guid UserId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC8");
        private static readonly Guid AccountingGroupId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC9");
        private static readonly Guid PointOfSaleId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DCA");
        private static readonly Guid AuthenticationMeansId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DCB");
        private static readonly Guid ProductId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC2");
        private static readonly Guid OfferId = Guid.Parse("73120441-86E2-661C-81CC-EF3A38883DC2");

        private const decimal Cost = 333;
        private const decimal Quantity = 2;

        private readonly Mock<IUpdateSale> _command;
        private IUpdateSale Cmd => _command.Object;

        public UpdateSaleHandlerTests()
        {
            _command = new Mock<IUpdateSale>();
            _command.SetupGet(c => c.Id).Returns(SaleId);
            _command.SetupGet(c => c.UserId).Returns(UserId);
            _command.SetupGet(c => c.Cost).Returns(Cost);
            _command.SetupGet(c => c.Quantity).Returns(Quantity);
            _command.SetupGet(c => c.AccountingGroupId).Returns(AccountingGroupId);
            _command.SetupGet(c => c.PointOfSaleId).Returns(PointOfSaleId);
            _command.SetupGet(c => c.AuthenticationMeansId).Returns(AuthenticationMeansId);
            _command.SetupGet(c => c.ProductId).Returns(ProductId);
            _command.SetupGet(c => c.OfferId).Returns(OfferId);
        }

        private bool IsExpectedSale(Accounting.Domain.Sale p)
        {
            Assert.AreEqual(Cmd.UserId, p.UserId);
            Assert.AreEqual(Cmd.Cost, p.Cost);
            Assert.AreEqual(Cmd.Quantity, p.Quantity);
            Assert.AreEqual(Cmd.AccountingGroupId, p.AccountingGroupId);
            Assert.AreEqual(Cmd.PointOfSaleId, p.PointOfSaleId);
            Assert.AreEqual(Cmd.AuthenticationMeansId, p.AuthenticationMeansId);
            Assert.AreEqual(Cmd.ProductId, p.ProductId);
            Assert.AreEqual(Cmd.OfferId, p.OfferId);
            return true;
        }

        private bool IsExpectedEvent(ISaleUpdated e)
        {
            Assert.AreEqual(Cmd.UserId, e.UserId);
            Assert.AreEqual(Cmd.Cost, e.Cost);
            Assert.AreEqual(Cmd.Quantity, e.Quantity);
            Assert.AreEqual(Cmd.AccountingGroupId, e.AccountingGroupId);
            Assert.AreEqual(Cmd.PointOfSaleId, e.PointOfSaleId);
            Assert.AreEqual(Cmd.AuthenticationMeansId, e.AuthenticationMeansId);
            Assert.AreEqual(Cmd.ProductId, e.ProductId);
            Assert.AreEqual(Cmd.OfferId, e.OfferId);
            return true;
        }

        [TestMethod]
        public void UpdatesSaleInRepository_RaisesIntegrationEvent()
        {
            var sale = new Accounting.Domain.Sale(Cmd.Id, 9, 9, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);

            var repository = new Mock<ISalesRepository>(MockBehavior.Strict);
            repository.Setup(r => r.GetAsync(Cmd.Id)).Returns(Task.FromResult(sale));
            repository.Setup(r => r.UpdateAsync(sale)).Returns(Task.CompletedTask).Verifiable();
            repository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();
            
            var publisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            publisher.Setup(p => p.Publish(It.Is<ISaleUpdated>(e => IsExpectedEvent(e)))).Returns(Task.CompletedTask).Verifiable();

            var userVerifier = new Mock<IUserVerifier>(MockBehavior.Strict);
            userVerifier.Setup(v => v.AssertExists(Cmd.UserId)).Returns(Task.CompletedTask).Verifiable();

            var agVerifier = new Mock<IAccountingGroupVerifier>(MockBehavior.Strict);
            agVerifier.Setup(v => v.AssertExists(Cmd.AccountingGroupId)).Returns(Task.CompletedTask).Verifiable();

            var posVerifier = new Mock<IPointOfSaleVerifier>(MockBehavior.Strict);
            posVerifier.Setup(v => v.AssertExists(Cmd.PointOfSaleId)).Returns(Task.CompletedTask).Verifiable();

            var meansVerifier = new Mock<IAuthenticationMeansVerifier>(MockBehavior.Strict);
            meansVerifier.Setup(v => v.AssertExists(Cmd.AuthenticationMeansId)).Returns(Task.CompletedTask).Verifiable();

            var productVerifier = new Mock<IProductVerifier>(MockBehavior.Strict);
            productVerifier.Setup(v => v.AssertExists(Cmd.ProductId)).Returns(Task.CompletedTask).Verifiable();

            var offerVerifier = new Mock<IOfferVerifier>(MockBehavior.Strict);
            offerVerifier.Setup(v => v.AssertExists(Cmd.OfferId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new UpdateSaleHandler(repository.Object, publisher.Object, agVerifier.Object, userVerifier.Object, posVerifier.Object, meansVerifier.Object, productVerifier.Object, offerVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();
            Assert.IsTrue(result.Successful);

            IsExpectedSale(sale);
            repository.Verify();
            publisher.Verify();
            userVerifier.Verify();
            agVerifier.Verify();
            posVerifier.Verify();
            meansVerifier.Verify();
            productVerifier.Verify();
            offerVerifier.Verify();
        }
    }
}
