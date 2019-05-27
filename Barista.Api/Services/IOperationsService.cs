using System;
using System.Threading.Tasks;
using Barista.Api.Models.Operation;
using RestEase;

namespace Barista.Api.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IOperationsService
    {
        [AllowAnyStatusCode]
        [Get("api/operations/{id}")]
        Task<Operation> GetOperation([Path] Guid id);
    }
}
