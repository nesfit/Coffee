using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Accounting.Services;
using Barista.Common;

namespace Barista.Accounting.Verifiers
{
    public class OfferVerifier : ExistenceVerifierBase<Guid>, IOfferVerifier
    {
        private readonly IOffersService _service;

        public OfferVerifier(IOffersService service) : base("offer", "offer")
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        protected override async Task<HttpResponseMessage> MakeRequest(Guid id)
            => await _service.StatOffer(id);
    }
}
