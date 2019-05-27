using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.Dto;
using Barista.Consistency.Models;
using RestEase;

namespace Barista.Consistency.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IPointsOfSaleService
    {
        [AllowAnyStatusCode]
        [Head("api/pointsOfSale/{pointOfSaleId}")]
        Task<HttpResponseMessage> StatPointOfSale([Path] Guid pointOfSaleId);

        [AllowAnyStatusCode]
        [Get("api/pointsOfSale/{id}/authorizedUsers")]
        Task<ResultPage<PointOfSaleAuthorizedUser>> BrowseAuthorizedUsers([Path] Guid id);

        [AllowAnyStatusCode]
        [Get("api/pointsOfSale/userAuthorizations/{userId}")]
        Task<ResultPage<PointOfSaleAuthorizedUser>> BrowseUserAuthorizations([Path] Guid userId, [Query] PagedQuery query);
    }
}
