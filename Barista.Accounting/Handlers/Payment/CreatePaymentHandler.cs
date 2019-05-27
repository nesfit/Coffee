using System;
using System.Threading.Tasks;
using Barista.Accounting.Events.Payment;
using Barista.Accounting.Repositories;
using Barista.Accounting.Verifiers;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Payment;

namespace Barista.Accounting.Handlers.Payment
{
    public class CreatePaymentHandler : ICommandHandler<ICreatePayment, IIdentifierResult>
    {
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly IUserVerifier _userVerifier;

        public CreatePaymentHandler(IPaymentsRepository paymentsRepository, IBusPublisher busPublisher, IUserVerifier userVerifier)
        {
            _paymentsRepository = paymentsRepository ?? throw new ArgumentNullException(nameof(paymentsRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _userVerifier = userVerifier ?? throw new ArgumentNullException(nameof(userVerifier));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreatePayment command, ICorrelationContext context)
        {
            await _userVerifier.AssertExists(command.UserId);

            var payment = new Domain.Payment(command.Id, command.Amount, command.UserId, command.Source, command.ExternalId);
            await _paymentsRepository.AddAsync(payment);

            try
            {
                await _paymentsRepository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("payment_already_exists", $"A payment with the ID '{command.Id}' already exists.");
            }

            await _busPublisher.Publish(new PaymentCreated(payment));

            return new IdentifierResult(payment.Id);
        }
    }
}
