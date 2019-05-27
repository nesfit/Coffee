using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestEase;

namespace Barista.StockItems.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IPointsOfSaleService
    {
        [AllowAnyStatusCode]
        [Head("api/pointsOfSale/{pointOfSaleId}")]
        Task<HttpResponseMessage> StatPointOfSale([Path] Guid pointOfSaleId);
    }
}
