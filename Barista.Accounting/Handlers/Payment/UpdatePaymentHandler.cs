using System;
using System.Threading.Tasks;
using Barista.Accounting.Events.Payment;
using Barista.Accounting.Repositories;
using Barista.Accounting.Verifiers;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Payment;

namespace Barista.Accounting.Handlers.Payment
{
    public class UpdatePaymentHandler : ICommandHandler<IUpdatePayment, IOperationResult>
    {
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly IUserVerifier _userVerifier;

        public UpdatePaymentHandler(IPaymentsRepository paymentsRepository, IBusPublisher busPublisher, IUserVerifier userVerifier)
        {
            _paymentsRepository = paymentsRepository ?? throw new ArgumentNullException(nameof(paymentsRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _userVerifier = userVerifier ?? throw new ArgumentNullException(nameof(userVerifier));
        }

        public async Task<IOperationResult> HandleAsync(IUpdatePayment command, ICorrelationContext context)
        {
            var payment = await _paymentsRepository.GetAsync(command.Id);
            if (payment is null)
                throw new BaristaException("payment_not_found", $"Payment with ID '{command.Id}' was not found");

            await _userVerifier.AssertExists(command.UserId);

            payment.SetUserId(command.UserId);
            payment.SetAmount(command.Amount);
            payment.SetExternalId(command.ExternalId);
            payment.SetSource(command.Source);

            await _paymentsRepository.UpdateAsync(payment);
            await _paymentsRepository.SaveChanges();
            await _busPublisher.Publish(new PaymentUpdated(payment));

            return OperationResult.Ok();
        }
    }
}
