using System;
using System.Threading.Tasks;
using Barista.Swipe.Models;
using RestEase;

namespace Barista.Swipe.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface ISaleStrategiesService
    {
        [AllowAnyStatusCode]
        [Get("api/saleStrategies/{id}")]
        Task<SaleStrategy> GetSaleStrategy([Path] Guid id);
    }
}
