using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestEase;

namespace Barista.PointsOfSale.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface ISaleStrategiesService
    {
        [AllowAnyStatusCode]
        [Head("api/saleStrategies/{saleStrategyId}")]
        Task<HttpResponseMessage> StatSaleStrategy([Path] Guid saleStrategyId);
    }
}
