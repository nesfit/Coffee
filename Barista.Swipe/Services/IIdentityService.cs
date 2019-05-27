using System;
using System.Threading.Tasks;
using Barista.Swipe.Models;
using Barista.Swipe.Queries;
using RestEase;

namespace Barista.Swipe.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IIdentityService
    {
        [AllowAnyStatusCode]
        [Get("api/assignmentsToUser/{id}")]
        Task<AssignmentToUser> GetAssignmentToUser([Path] Guid id);
    }
}
