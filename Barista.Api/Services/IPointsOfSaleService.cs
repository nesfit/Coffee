using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Api.Models.PointsOfSale;
using Barista.Api.Queries;
using Barista.Common.Dto;
using RestEase;

namespace Barista.Api.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IPointsOfSaleService
    {
        [AllowAnyStatusCode]
        [Get("api/pointsOfSale")]
        Task<ResultPage<PointOfSale>> BrowsePointsOfSale([Query] BrowsePointsOfSale query);

        [AllowAnyStatusCode]
        [Get("api/pointsOfSale/{id}")]
        Task<PointOfSale> GetPointOfSale([Path] Guid id);

        [AllowAnyStatusCode]
        [Head("api/pointsOfSale/{id}")]
        Task<HttpResponseMessage> StatPointOfSale([Path] Guid id);

        [AllowAnyStatusCode]
        [Get("api/pointsOfSale/{id}/authorizedUsers")]
        Task<ResultPage<PointOfSaleAuthorizedUser>> BrowseAuthorizedUsers([Path] Guid id, [Query] BrowsePointOfSaleAuthorizedUsers query);

        [AllowAnyStatusCode]
        [Get("api/pointsOfSale/{id}/authorizedUsers/{userId}")]
        Task<PointOfSaleAuthorizedUser> GetAuthorizedUser([Path] Guid id, [Path] Guid userId);

        [Head("api/pointsOfSale/authorizedUsers/{userId}")]
        Task<HttpResponseMessage> FindAuthorizedUser([Path] Guid userId);
    }
}
