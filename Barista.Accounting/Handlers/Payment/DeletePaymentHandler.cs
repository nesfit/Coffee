using System;
using System.Threading.Tasks;
using Barista.Accounting.Events.Payment;
using Barista.Accounting.Repositories;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Payment;

namespace Barista.Accounting.Handlers.Payment
{
    public class DeletePaymentHandler : ICommandHandler<IDeletePayment, IOperationResult>
    {
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IBusPublisher _busPublisher;

        public DeletePaymentHandler(IPaymentsRepository paymentsRepository, IBusPublisher busPublisher)
        {
            _paymentsRepository = paymentsRepository ?? throw new ArgumentNullException(nameof(paymentsRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeletePayment command, ICorrelationContext context)
        {
            var payment = await _paymentsRepository.GetAsync(command.Id);
            if (payment is null)
                throw new BaristaException("payment_not_found", $"Could not find payment with ID '{command.Id}'");

            await _paymentsRepository.DeleteAsync(payment);
            await _paymentsRepository.SaveChanges();
            await _busPublisher.Publish(new PaymentDeleted(command.Id));

            return OperationResult.Ok();
        }
    }
}
