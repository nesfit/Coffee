using System;
using System.Threading.Tasks;
using Barista.Api.Models.SaleStrategies;
using Barista.Api.Queries;
using Barista.Common.Dto;
using RestEase;

namespace Barista.Api.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface ISaleStrategiesService
    {
        [AllowAnyStatusCode]
        [Get("api/saleStrategies")]
        Task<ResultPage<SaleStrategy>> BrowseSaleStrategies([Query] DisplayNameQuery query);

        [AllowAnyStatusCode]
        [Get("api/saleStrategies/{id}")]
        Task<SaleStrategy> GetSaleStrategy([Path] Guid id);
    }
}
