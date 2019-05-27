using System;
using System.Threading.Tasks;
using Barista.SaleStrategies.Models;
using RestEase;

namespace Barista.SaleStrategies.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IAccountingService
    {
        [Get("api/balance/{userId}")]
        Task<UserBalance> GetBalance([Path] Guid userId);
    }
}
