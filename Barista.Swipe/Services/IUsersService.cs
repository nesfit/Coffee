using System;
using System.Threading.Tasks;
using Barista.Swipe.Models;
using RestEase;

namespace Barista.Swipe.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IUsersService
    {
        [AllowAnyStatusCode]
        [Get("api/users/{id}")]
        Task<User> GetUser([Path] Guid id);
    }
}
