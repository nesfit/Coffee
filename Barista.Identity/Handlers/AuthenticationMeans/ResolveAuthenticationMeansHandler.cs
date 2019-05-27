using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.Dispatchers;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AuthenticationMeans;
using Barista.Identity.Queries;

namespace Barista.Identity.Handlers.AuthenticationMeans
{
    public class ResolveAuthenticationMeansHandler : ICommandHandler<IResolveAuthenticationMeans, IIdentifierResult>
    {
        private readonly IDispatcher _dispatcher;

        public ResolveAuthenticationMeansHandler(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        public async Task<IIdentifierResult> HandleAsync(IResolveAuthenticationMeans command, ICorrelationContext correlationContext)
        {
            var queryResults = await _dispatcher.QueryAsync(new BrowseAuthenticationMeans {Method = command.Method, Value = command.Value, ResultsPerPage = 1 });
            if (queryResults.TotalResults != 1)
                return new IdentifierResult("authentication_means_not_found", $"Could not find authentication means of method '{command.Method}' and value '{command.Value}");

            return new IdentifierResult(queryResults.Items.Single().Id);
        }
    }
}
