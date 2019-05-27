using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestEase;

namespace Barista.StockOperations.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IAccountingService
    {
        [AllowAnyStatusCode]
        [Head("api/sales/{saleId}")]
        Task<HttpResponseMessage> StatSale([Path] Guid saleId);
    }
}
