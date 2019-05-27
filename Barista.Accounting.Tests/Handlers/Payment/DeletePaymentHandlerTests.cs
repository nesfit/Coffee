using System;
using System.Threading.Tasks;
using Barista.Accounting.Handlers.Payment;
using Barista.Accounting.Repositories;
using Barista.Common;
using Barista.Contracts.Commands.Payment;
using Barista.Contracts.Events.Payment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Accounting.Tests.Handlers.Payment
{
    [TestClass]
    public class DeletePaymentHandlerTests
    {
        private static readonly Guid PaymentGuid = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");

        private readonly Mock<IDeletePayment> _command;
        private IDeletePayment Cmd => _command.Object;
        
        public DeletePaymentHandlerTests()
        {
            _command = new Mock<IDeletePayment>();
            _command.SetupGet(c => c.Id).Returns(PaymentGuid);
        }

        private bool IsExpectedEvent(IPaymentDeleted e)
        {
            Assert.AreEqual(Cmd.Id, e.Id);
            return true;
        }

        [TestMethod]
        public void DeletesPaymentFromRepository_RaisesIntegrationEvent()
        {
            var payment = new Accounting.Domain.Payment(PaymentGuid, 1, Guid.Empty, "test", null);

            var paymentsRepository = new Mock<IPaymentsRepository>(MockBehavior.Strict);
            paymentsRepository.Setup(r => r.GetAsync(payment.Id)).Returns(Task.FromResult(payment)).Verifiable();
            paymentsRepository.Setup(r => r.DeleteAsync(payment)).Returns(Task.CompletedTask).Verifiable();
            paymentsRepository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IPaymentDeleted>(e => IsExpectedEvent(e)))).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeletePaymentHandler(paymentsRepository.Object, busPublisher.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();
            Assert.IsTrue(result.Successful);

            paymentsRepository.Verify();
            busPublisher.Verify();
        }
    }
}
