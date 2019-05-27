using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Consistency.Commands;
using Barista.Contracts.Commands.AuthenticationMeans;
using MassTransit.Courier;

namespace Barista.Consistency.Activities.Initialization
{
    public class CreatePasswordActivity : Activity<UserIdParameters, GuidLog>
    {
        private readonly IBusPublisher _busPublisher;

        public CreatePasswordActivity(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<UserIdParameters> context)
        {
            var result = await _busPublisher.SendRequest<ICreateAuthenticationMeans, IIdentifierResult>(
                new CreateAuthenticationMeans
                {
                    Id = Guid.NewGuid(),
                    Method = "password",
                    ValidSince = DateTimeOffset.UtcNow,
                    Value = "AQAAAAEAACcQAAAAEMBltRflZWAQYQiGzPsZ6Hp4x10Pv4d4uJP1GSr5DOdwYuhqXzUmTyAX8FgUuPssig=="
                }
            );

            if (!result.Successful)
                throw result.ToException();

            var meansId = result.Id.Value;
            return context.CompletedWithVariables(new GuidLog {Id = meansId}, new UserIdMeansIdParameters
            {
                UserId = context.Arguments.UserId,
                MeansId = meansId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<GuidLog> context)
        {
            var result = await _busPublisher.SendRequest(new DeleteAuthenticationMeans {Id = context.Log.Id});
            if (!result.Successful)
                throw result.ToException();

            return context.Compensated();
        }
    }
}
