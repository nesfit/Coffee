using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Consistency.Commands;
using Barista.Consistency.Repositories;

namespace Barista.Consistency.Handlers
{
    public class DeleteScheduledEventHandler : ICommandHandler<DeleteScheduledEvent, IOperationResult>
    {
        private readonly IScheduledEventRepository _repository;

        public DeleteScheduledEventHandler(IScheduledEventRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IOperationResult> HandleAsync(DeleteScheduledEvent command, ICorrelationContext correlationContext)
        {
            var subject = await _repository.GetAsync(command.Id);
            if (subject != null)
            {
                await _repository.DeleteAsync(subject);
                await _repository.SaveChanges();
            }

            return OperationResult.Ok();
        }
    }
}
