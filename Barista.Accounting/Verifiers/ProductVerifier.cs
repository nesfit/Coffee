using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Accounting.Services;
using Barista.Common;

namespace Barista.Accounting.Verifiers
{
    public class ProductVerifier : ExistenceVerifierBase<Guid>, IProductVerifier
    {
        private readonly IProductsService _service;

        public ProductVerifier(IProductsService service) : base("product", "product")
        {
            _service = service;
        }

        protected override async Task<HttpResponseMessage> MakeRequest(Guid id)
            => await _service.StatProduct(id);
    }
}
