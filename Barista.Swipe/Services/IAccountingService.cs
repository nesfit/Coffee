using System;
using System.Threading.Tasks;
using Barista.Swipe.Models;
using RestEase;

namespace Barista.Swipe.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IAccountingService
    {
        [AllowAnyStatusCode]
        [Get("api/sales/{id}")]
        Task<Sale> GetSale([Path] Guid id);

        [AllowAnyStatusCode]
        [Get("api/spending/ofMeans/{meansId}")]
        Task<decimal> GetSpendingOfMeans([Path] Guid meansId);

        [AllowAnyStatusCode]
        [Get("api/spending/ofMeans/{meansId}/since/{since}")]
        Task<decimal> GetSpendingOfMeans([Path] Guid meansId, [Path] DateTimeOffset since);
    }
}
