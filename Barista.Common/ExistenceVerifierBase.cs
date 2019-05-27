using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Barista.Common
{
    public abstract class ExistenceVerifierBase<TEntityId> : IExistenceVerifier<TEntityId>
    {
        private readonly string _humanReadableEntityName;
        private readonly string _errorCodeEntityName;

        protected ExistenceVerifierBase(string errorCodeEntityName, string humanReadableEntityName)
        {
            if (string.IsNullOrEmpty(errorCodeEntityName))
                throw new ArgumentException("Value cannot be null or empty.", nameof(errorCodeEntityName));
            if (string.IsNullOrWhiteSpace(humanReadableEntityName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(humanReadableEntityName));

            _errorCodeEntityName = errorCodeEntityName;
            _humanReadableEntityName = humanReadableEntityName;
        }

        protected abstract Task<HttpResponseMessage> MakeRequest(TEntityId id); 

        public async Task AssertExists(TEntityId entityId)
        {
            var response = await MakeRequest(entityId);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new BaristaException($"{_errorCodeEntityName}_not_found", $"Could not find {_humanReadableEntityName} with ID '{entityId}'");
            else if (!response.IsSuccessStatusCode) // TODO log the response somewhere
                throw new BaristaException($"{_errorCodeEntityName}_verification_failed", "Could not verify existence of {_humanReadableEntityName} due to an internal error.");
        }
    }
}
