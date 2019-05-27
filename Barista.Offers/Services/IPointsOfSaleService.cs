using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Common.Dto;
using RestEase;

namespace Barista.Offers.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IPointsOfSaleService
    {
        [AllowAnyStatusCode]
        [Head("api/pointsOfSale/{pointOfSaleId}")]
        Task<HttpResponseMessage> StatPointOfSale([Path] Guid pointOfSaleId);
    }
}
