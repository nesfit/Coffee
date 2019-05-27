using System;
using System.Threading.Tasks;
using Barista.Accounting.Handlers.Payment;
using Barista.Accounting.Repositories;
using Barista.Accounting.Verifiers;
using Barista.Common;
using Barista.Contracts.Commands.Payment;
using Barista.Contracts.Events.Payment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Accounting.Tests.Handlers.Payment
{
    [TestClass]
    public class UpdatePaymentHandlerTests
    {
        private static readonly Guid PaymentGuid = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");

        private static readonly Guid NewPaymentUserId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC8");
        private const decimal NewPaymentAmount = 333;
        private const string NewPaymentSource = "Src";
        private const string NewPaymentExternalId = "ExtId";

        private readonly Mock<IUpdatePayment> _command;
        private IUpdatePayment Cmd => _command.Object;


        public UpdatePaymentHandlerTests()
        {
            _command = new Mock<IUpdatePayment>();
            _command.SetupGet(c => c.Id).Returns(PaymentGuid);
            _command.SetupGet(c => c.UserId).Returns(NewPaymentUserId);
            _command.SetupGet(c => c.Amount).Returns(NewPaymentAmount);
            _command.SetupGet(c => c.Source).Returns(NewPaymentSource);
            _command.SetupGet(c => c.ExternalId).Returns(NewPaymentExternalId);
        }

        private void IsExpectedPayment(Accounting.Domain.Payment p)
        {
            Assert.AreEqual(Cmd.UserId, p.UserId);
            Assert.AreEqual(Cmd.Amount, p.Amount);
            Assert.AreEqual(Cmd.Source, p.Source);
            Assert.AreEqual(Cmd.ExternalId, p.ExternalId);
        }

        private bool IsExpectedEvent(IPaymentUpdated e)
        {
            Assert.AreEqual(Cmd.UserId, e.UserId);
            Assert.AreEqual(Cmd.Amount, e.Amount);
            Assert.AreEqual(Cmd.Source, e.Source);
            Assert.AreEqual(Cmd.ExternalId, e.ExternalId);
            return true;
        }

        [TestMethod]
        public void UpdatesPaymentInRepository_RaisesIntegrationEvent()
        {
            var payment = new Accounting.Domain.Payment(PaymentGuid, 1, Guid.Empty, "test", null);

            var paymentsRepository = new Mock<IPaymentsRepository>(MockBehavior.Strict);
            paymentsRepository.Setup(r => r.GetAsync(payment.Id)).Returns(Task.FromResult(payment)).Verifiable();
            paymentsRepository.Setup(r => r.UpdateAsync(payment)).Returns(Task.CompletedTask).Verifiable();
            paymentsRepository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IPaymentUpdated>(e => IsExpectedEvent(e)))).Returns(Task.CompletedTask).Verifiable();

            var userVerifier = new Mock<IUserVerifier>(MockBehavior.Strict);
            userVerifier.Setup(v => v.AssertExists(Cmd.UserId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new UpdatePaymentHandler(paymentsRepository.Object, busPublisher.Object, userVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();
            Assert.IsTrue(result.Successful);

            IsExpectedPayment(payment);
            paymentsRepository.Verify();
            busPublisher.Verify();
            userVerifier.Verify();
        }
    }
}
