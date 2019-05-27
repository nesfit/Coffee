using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Barista.Common.RestEase
{
    public class FabioMessageHandler : DelegatingHandler
    {
        private readonly IFabioOptions _options;

        public FabioMessageHandler(IFabioOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(_options.Url))
                throw new InvalidOperationException("Fabio URL was not provided.");
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //request.RequestUri = OverrideRequestUri(request.RequestUri);

            return await Policy.Handle<Exception>()
                .WaitAndRetryAsync(RequestRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await base.SendAsync(request, cancellationToken));
        }

        private int RequestRetries => _options.RequestRetries <= 0 ? 3 : _options.RequestRetries;

        /*private Uri OverrideRequestUri(Uri requestUri)
        {
            var newUri = _options.Url;
            var oldUri = requestUri.OriginalString;
            if (!oldUri.StartsWith('/'))
                newUri += '/';
            newUri += oldUri;
            return new Uri(newUri);
        }*/
    }
}
