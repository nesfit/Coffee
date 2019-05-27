using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestEase;

namespace Barista.Accounting.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IIdentityService
    {
        [AllowAnyStatusCode]
        [Head("api/authenticationMeans/{meansId}")]
        Task<HttpResponseMessage> StatAuthenticationMeans([Path] Guid meansId);
    }
}
