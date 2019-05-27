using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestEase;

namespace Barista.StockOperations.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IStockItemsService
    {
        [AllowAnyStatusCode]
        [Head("api/stockItems/{stockItemId}")]
        Task<HttpResponseMessage> StatStockItem([Path] Guid stockItemId);
    }
}
