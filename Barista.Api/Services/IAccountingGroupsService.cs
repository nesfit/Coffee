using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Api.Models.AccountingGroups;
using Barista.Api.Queries;
using Barista.Common.Dto;
using RestEase;

namespace Barista.Api.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IAccountingGroupsService
    {
        [AllowAnyStatusCode]
        [Get("api/accountingGroups")]
        Task<ResultPage<AccountingGroup>> BrowseAccountingGroups([Query] DisplayNameQuery query);

        [AllowAnyStatusCode]
        [Get("api/accountingGroups/{id}")]
        Task<AccountingGroup> GetAccountingGroup([Path] Guid id);

        [AllowAnyStatusCode]
        [Get("api/accountingGroups/{id}/authorizedUsers")]
        Task<ResultPage<AccountingGroupAuthorizedUser>> BrowseAuthorizedUsers([Path] Guid id, [Query] BrowseAccountingGroupAuthorizedUsers query);

        [AllowAnyStatusCode]
        [Get("api/accountingGroups/{id}/authorizedUsers/{userId}")]
        Task<AccountingGroupAuthorizedUser> GetAuthorizedUser([Path] Guid id, [Path] Guid userId);

        [Head("api/accountingGroups/authorizedUsers/{userId}")]
        Task<HttpResponseMessage> FindAuthorizedUser([Path] Guid userId);
    }
}
