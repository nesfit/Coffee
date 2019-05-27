using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Common;
using Barista.StockOperations.Services;

namespace Barista.StockOperations.Verifiers
{
    public class SaleVerifier : ExistenceVerifierBase<Guid>, ISaleVerifier
    {
        private readonly IAccountingService _service;

        public SaleVerifier(IAccountingService service)
            : base("sale", "sale")
        {
            _service = service;
        }

        protected override async Task<HttpResponseMessage> MakeRequest(Guid id)
            => await _service.StatSale(id);
    }
}
