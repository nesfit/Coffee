using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Api.Models.Identity;
using Barista.Api.Queries;
using Barista.Common;
using Barista.Common.Dto;
using RestEase;

namespace Barista.Api.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IIdentityService
    {
        [Get("api/assignmentsToPointOfSale")]
        Task<ResultPage<AssignmentToPointOfSale>> BrowseAssignmentsToPointOfSale([Query] BrowseAssignmentsToPointOfSale query);

        [AllowAnyStatusCode]
        [Get("api/assignmentsToPointOfSale/{id}")]
        Task<AssignmentToPointOfSale> GetAssignmentToPointOfSale([Path] Guid id);

        [Get("api/assignmentsToUser")]
        Task<ResultPage<AssignmentToUser>> BrowseAssignmentsToUser([Query] BrowseAssignmentsToUser query);

        [AllowAnyStatusCode]
        [Get("api/assignmentsToUser/{id}")]
        Task<AssignmentToUser> GetAssignmentToUser([Path] Guid id);

        [Get("api/authenticationMeans")]
        Task<ResultPage<AuthenticationMeans>> BrowseAuthenticationMeans([Query] BrowseAuthenticationMeans query);

        [AllowAnyStatusCode]
        [Get("api/authenticationMeans/{id}")]
        Task<AuthenticationMeans> GetAuthenticationMeans([Path] Guid id);

        [AllowAnyStatusCode]
        [Head("api/authenticationMeans/{id}")]
        Task<HttpResponseMessage> StatAuthenticationMeans([Path] Guid id);

        [Get("api/assignmentsToUser/{userAssignmentId}/spendingLimits")]
        Task<ResultPage<SpendingLimit>> BrowseSpendingLimits([Path] Guid userAssignmentId, [Query] PagedQuery query);

        [AllowAnyStatusCode]
        [Get("api/assignmentToUser/{userAssignmentId}/spendingLimits/{spendingLimitId}")]
        Task<SpendingLimit> GetSpendingLimit([Path] Guid userAssignmentId, [Path] Guid spendingLimitId);

        [Get("api/assignedMeans/toUser/{userId}")]
        Task<ResultPage<AuthenticationMeansWithValue>> BrowseMeansAssignedToUser([Path] Guid userId, [Query] BrowseAssignedMeans query);

        [Get("api/assignedMeans/toUser/{userId}")]
        Task<ResultPage<AuthenticationMeans>> BrowseMeansAssignedToUserNoValue([Path] Guid userId, [Query] BrowseAssignedMeans query);

        [Get("api/assignedMeans/toPointOfSale/{pointOfSaleId}")]
        Task<ResultPage<AuthenticationMeansWithValue>> BrowseMeansAssignedToPointOfSale([Path] Guid pointOfSaleId, [Query] BrowseAssignedMeans query);

        [Get("api/assignedMeans/toPointOfSale/{pointOfSaleId}")]
        Task<ResultPage<AuthenticationMeans>> BrowseMeansAssignedToPointOfSaleNoValue([Path] Guid pointOfSaleId, [Query] BrowseAssignedMeans query);
    }
}
