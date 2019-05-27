using System;
using System.Threading.Tasks;
using Barista.Api.Models.Accounting;
using Barista.Api.Queries;
using Barista.Common;
using Barista.Common.Dto;
using RestEase;

namespace Barista.Api.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IAccountingService
    {
        [AllowAnyStatusCode]
        [Get("api/payments")]
        Task<ResultPage<Payment>> BrowsePayments([Query] BrowsePayments query);

        [AllowAnyStatusCode]
        [Get("api/payments/{id}")]
        Task<Payment> GetPayment([Path] Guid id);

        [Get("api/sales")]
        Task<ResultPage<Sale>> BrowseSales([Query] BrowseSales query);

        [AllowAnyStatusCode]
        [Get("api/sales/{id}")]
        Task<Sale> GetSale([Path] Guid id);

        [Get("api/sales/{saleId}/stateChanges/")]
        Task<ResultPage<SaleStateChange>> BrowseSaleStateChanges([Path] Guid saleId, [Query] PagedQuery query);

        [AllowAnyStatusCode]
        [Get("api/sales/{saleId}/stateChanges/{saleStateChangeId}")]
        Task<SaleStateChange> GetSaleStateChange([Path] Guid saleId, [Path] Guid saleStateChangeId);

        [Get("api/balance/{userId}")]
        Task<UserBalance> GetBalance([Path] Guid userId);

        [Get("api/spending/ofUser/{userId}")]
        Task<ResultPage<SpendingOfUser>> BrowseSpendingOfUser([Path] Guid userId, [Query] BrowseSpendingOfUser query);

        [Get("api/spending/ofUser/{userId}/sum")]
        Task<decimal> SumSpendingOfUser([Path] Guid userId, [Query] BrowseSpendingOfUser query);
    }
}
