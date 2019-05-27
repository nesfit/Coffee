using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.Dto;
using Barista.Mqtt.Models;
using RestEase;

namespace Barista.Mqtt.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IPointsOfSaleService
    {
        [AllowAnyStatusCode]
        [Get("api/pointsOfSale")]
        Task<ResultPage<PointOfSale>> BrowsePointsOfSale([Query] PagedQuery query);

        [AllowAnyStatusCode]
        [Get("api/pointsOfSale/{id}")]
        Task<PointOfSale> GetPointOfSale([Path] Guid id);
    }
}
