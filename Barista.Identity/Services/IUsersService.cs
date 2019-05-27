using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Identity.Models;
using RestEase;

namespace Barista.Identity.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IUsersService
    {
        [AllowAnyStatusCode]
        [Head("api/users/{userId}")]
        Task<HttpResponseMessage> StatUser([Path] Guid userId);

        [AllowAnyStatusCode]
        [Get("api/users/{userId}")]
        Task<User> GetUser([Path] Guid userId);
    }
}
