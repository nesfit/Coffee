using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.Dto;
using Barista.Consistency.Models;
using Barista.Consistency.Queries;
using RestEase;

namespace Barista.Consistency.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IAccountingGroupsService
    {
        [AllowAnyStatusCode]
        [Get("api/accountingGroups/{id}/authorizedUsers")]
        Task<ResultPage<AccountingGroupAuthorizedUser>> BrowseAuthorizedUsers([Path] Guid id);

        [AllowAnyStatusCode]
        [Get("api/accountingGroups/userAuthorizations/{userId}")]
        Task<ResultPage<AccountingGroupAuthorizedUser>> BrowseUserAuthorizations([Path] Guid userId, [Query] PagedQuery query);
    }
}
