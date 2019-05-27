using System;
using System.Threading.Tasks;
using Barista.Swipe.Models;
using Barista.Swipe.Queries;
using RestEase;

namespace Barista.Swipe.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IOffersService
    {
        [AllowAnyStatusCode]
        [Get("api/offers/{id}")]
        Task<Offer> GetOffer([Path]Guid id);
    }
}
