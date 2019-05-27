using System;
using System.Threading.Tasks;
using Barista.Api.Models.Products;
using Barista.Api.Queries;
using Barista.Common.Dto;
using RestEase;

namespace Barista.Api.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IProductsService
    {
        [AllowAnyStatusCode]
        [Get("api/products")]
        Task<ResultPage<Product>> BrowseProducts([Query] DisplayNameQuery query);

        [AllowAnyStatusCode]
        [Get("api/products/{id}")]
        Task<Product> GetProduct([Path] Guid id);
    }
}
