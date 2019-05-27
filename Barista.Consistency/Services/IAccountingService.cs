using System;
using System.Threading.Tasks;
using Barista.Consistency.Models;
using RestEase;

namespace Barista.Consistency.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IAccountingService
    {
        [AllowAnyStatusCode]
        [Get("api/sales/{id}")]
        Task<Sale> GetSale([Path] Guid id);
    }
}
