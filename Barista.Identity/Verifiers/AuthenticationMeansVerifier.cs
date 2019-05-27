using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Identity.Repositories;

namespace Barista.Identity.Verifiers
{
    public class AuthenticationMeansVerifier : IAuthenticationMeansVerifier
    {
        private readonly IAuthenticationMeansRepository _repository;

        public AuthenticationMeansVerifier(IAuthenticationMeansRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task AssertExists(Guid entityId)
        {
            var means = await _repository.GetAsync(entityId);
            if (means is null)
                throw new BaristaException("authentication_means_not_found",
                    $"Could not find authentication means with ID '{entityId}'");
        }
    }
}
