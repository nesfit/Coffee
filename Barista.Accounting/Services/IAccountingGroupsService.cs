using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestEase;

namespace Barista.Accounting.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IAccountingGroupsService
    {
        [AllowAnyStatusCode]
        [Head("api/accountingGroups/{accountingGroupId}")]
        Task<HttpResponseMessage> StatAccountingGroup([Path] Guid accountingGroupId);
    }
}
