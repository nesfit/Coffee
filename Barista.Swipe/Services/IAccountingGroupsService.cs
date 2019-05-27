using System;
using System.Threading.Tasks;
using Barista.Swipe.Models;
using RestEase;

namespace Barista.Swipe.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IAccountingGroupsService
    {
        [AllowAnyStatusCode]
        [Get("api/accountingGroups/{id}")]
        Task<AccountingGroup> GetAccountingGroup([Path] Guid id);
    }
}
