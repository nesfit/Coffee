using System.Threading.Tasks;
using Barista.Common.Dto;
using Barista.Mqtt.Models;
using Barista.Mqtt.Queries;
using RestEase;

namespace Barista.Mqtt.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IOffersService
    {
        [AllowAnyStatusCode]
        [Get("api/offers")]
        Task<ResultPage<Offer>> BrowseOffers([Query] BrowseOffers query);
    }
}
