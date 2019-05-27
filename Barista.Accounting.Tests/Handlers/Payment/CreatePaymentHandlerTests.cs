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
    public class CreatePaymentHandlerTests
    {
        private static readonly Guid NewPaymentGuid = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC7");
        private static readonly Guid NewPaymentUserId = Guid.Parse("73120441-86E2-4A1C-81CC-EF3A38883DC8");
        private const decimal NewPaymentAmount = 333;
        private const string NewPaymentSource = "Src";
        private const string NewPaymentExternalId = "ExtId";

        private readonly Mock<ICreatePayment> _command;
        private ICreatePayment Cmd => _command.Object;


        public CreatePaymentHandlerTests()
        {
            _command = new Mock<ICreatePayment>();
            _command.SetupGet(c => c.Id).Returns(NewPaymentGuid);
            _command.SetupGet(c => c.UserId).Returns(NewPaymentUserId);
            _command.SetupGet(c => c.Amount).Returns(NewPaymentAmount);
            _command.SetupGet(c => c.Source).Returns(NewPaymentSource);
            _command.SetupGet(c => c.ExternalId).Returns(NewPaymentExternalId);
        }

        private bool IsExpectedPayment(Accounting.Domain.Payment p)
        {
            Assert.AreEqual(Cmd.UserId, p.UserId);
            Assert.AreEqual(Cmd.Amount, p.Amount);
            Assert.AreEqual(Cmd.Source, p.Source);
            Assert.AreEqual(Cmd.ExternalId, p.ExternalId);
            return true;
        }

        private bool IsExpectedEvent(IPaymentCreated e)
        {
            Assert.AreEqual(Cmd.UserId, e.UserId);
            Assert.AreEqual(Cmd.Amount, e.Amount);
            Assert.AreEqual(Cmd.Source, e.Source);
            Assert.AreEqual(Cmd.ExternalId, e.ExternalId);
            return true;
        }

        [TestMethod]
        public void AddsPaymentToRepository_RaisesIntegrationEvent()
        {
            var paymentsRepository = new Mock<IPaymentsRepository>(MockBehavior.Strict);
            paymentsRepository.Setup(r => r.AddAsync(It.Is<Accounting.Domain.Payment>(p => IsExpectedPayment(p)))).Returns(Task.CompletedTask).Verifiable();
            paymentsRepository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask).Verifiable();

            var busPublisher = new Mock<IBusPublisher>(MockBehavior.Strict);
            busPublisher.Setup(p => p.Publish(It.Is<IPaymentCreated>(e => IsExpectedEvent(e)))).Returns(Task.CompletedTask).Verifiable();

            var userVerifier = new Mock<IUserVerifier>(MockBehavior.Strict);
            userVerifier.Setup(r => r.AssertExists(Cmd.UserId)).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreatePaymentHandler(paymentsRepository.Object, busPublisher.Object, userVerifier.Object);
            var result = handler.HandleAsync(Cmd, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();
            Assert.IsTrue(result.Successful);
            paymentsRepository.Verify();
            busPublisher.Verify();
            userVerifier.Verify();
        }
    }
}
