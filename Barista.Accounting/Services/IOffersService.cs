using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestEase;

namespace Barista.Accounting.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IOffersService
    {
        [AllowAnyStatusCode]
        [Head("api/offers/{offerId}")]
        Task<HttpResponseMessage> StatOffer([Path] Guid offerId);
    }
}
