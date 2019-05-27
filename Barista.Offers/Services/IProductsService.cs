using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestEase;

namespace Barista.Offers.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IProductsService
    {
        [AllowAnyStatusCode]
        [Head("api/products/{productId}")]
        Task<HttpResponseMessage> StatProduct([Path] Guid productId);
    }
}
