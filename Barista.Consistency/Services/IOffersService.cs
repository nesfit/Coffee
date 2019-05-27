using System;
using System.Threading.Tasks;
using Barista.Common.Dto;
using Barista.Consistency.Models;
using Barista.Consistency.Queries;
using RestEase;

namespace Barista.Consistency.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IOffersService
    {
        [AllowAnyStatusCode]
        [Get("api/offers")]
        Task<ResultPage<Offer>> BrowseOffers([Query] BrowseOffers query);


        [AllowAnyStatusCode]
        [Get("api/offers/{id}")]
        Task<Offer> GetOffer([Path] Guid id);
    }
}
