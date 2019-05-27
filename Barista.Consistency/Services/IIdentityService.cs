using System.Threading.Tasks;
using Barista.Common.Dto;
using Barista.Consistency.Models;
using Barista.Consistency.Queries;
using RestEase;

namespace Barista.Consistency.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IIdentityService
    {
        [AllowAnyStatusCode]
        [Get("api/assignmentsToPointOfSale")]
        Task<ResultPage<Assignment>> BrowseAssignmentsToPointOfSale([Query] BrowseAssignments query);


        [AllowAnyStatusCode]
        [Get("api/assignmentsToUser")]
        Task<ResultPage<Assignment>> BrowseAssignmentsToUser([Query] BrowseAssignments query);
    }
}
