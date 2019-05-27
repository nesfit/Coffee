using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Accounting.Services;
using Barista.Common;

namespace Barista.Accounting.Verifiers
{
    public class PointOfSaleVerifier : ExistenceVerifierBase<Guid>, IPointOfSaleVerifier
    {
        private readonly IPointsOfSaleService _service;

        public PointOfSaleVerifier(IPointsOfSaleService service)
            : base("point_of_sale", "point of sale")
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        protected override async Task<HttpResponseMessage> MakeRequest(Guid id)
            => await _service.StatPointOfSale(id);
    }
}
