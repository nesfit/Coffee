using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Accounting.Services;
using Barista.Common;

namespace Barista.Accounting.Verifiers
{
    public class AuthenticationMeansVerifier : ExistenceVerifierBase<Guid>, IAuthenticationMeansVerifier
    {
        private readonly IIdentityService _service;

        public AuthenticationMeansVerifier(IIdentityService service)
            : base("authentication_means", "authentication means")
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        protected override async Task<HttpResponseMessage> MakeRequest(Guid id)
            => await _service.StatAuthenticationMeans(id);
    }
}
