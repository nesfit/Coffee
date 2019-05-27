using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.AccountingGroups.Services;
using Barista.Common;

namespace Barista.AccountingGroups.Verifiers
{
    public class SaleStrategyVerifier : ExistenceVerifierBase<Guid>, ISaleStrategyVerifier
    {
        private readonly ISaleStrategiesService _service;

        public SaleStrategyVerifier(ISaleStrategiesService service)
            : base("sale_strategy", "sale strategy")
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        protected override async Task<HttpResponseMessage> MakeRequest(Guid id)
            => await _service.StatSaleStrategy(id);
    }
}
