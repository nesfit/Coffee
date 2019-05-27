using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Offers.Services;

namespace Barista.Offers.Verifiers
{
    public class ProductVerifier : ExistenceVerifierBase<Guid>, IProductVerifier
    {
        private readonly IProductsService _service;

        public ProductVerifier(IProductsService service)
            : base("product", "product")
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        protected override async Task<HttpResponseMessage> MakeRequest(Guid id)
            => await _service.StatProduct(id);
    }
}
