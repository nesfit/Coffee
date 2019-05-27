using System;
using System.Threading.Tasks;
using Barista.Swipe.Models;
using RestEase;

namespace Barista.Swipe.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IProductsService
    {
        [AllowAnyStatusCode]
        [Get("api/products/{id}")]
        Task<Product> GetProduct([Path] Guid id);
    }
}
