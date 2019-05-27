using System;
using System.Threading.Tasks;
using Barista.Api.Models.StockOperations;
using Barista.Api.Queries;
using Barista.Common.Dto;
using RestEase;

namespace Barista.Api.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IStockOperationsService
    {
        [AllowAnyStatusCode]
        [Get("api/stockOperations")]
        Task<ResultPage<StockOperation>> BrowseStockOperations([Query] BrowseStockOperations query);

        [AllowAnyStatusCode]
        [Get("api/stockOperations/{id}")]
        Task<StockOperation> GetStockOperation([Path] Guid id);

        [AllowAnyStatusCode]
        [Get("api/stockOperations/balance/{stockItemId}")]
        Task<decimal> GetStockItemBalance([Path] Guid stockItemId);

        [AllowAnyStatusCode]
        [Get("api/manualStockOperations")]
        Task<ResultPage<ManualStockOperation>> BrowseManualStockOperations([Query] BrowseStockOperations query);

        [AllowAnyStatusCode]
        [Get("api/manualStockOperations/{id}")]
        Task<ManualStockOperation> GetManualStockOperation([Path] Guid id);

        [AllowAnyStatusCode]
        [Get("api/saleBasedStockOperations")]
        Task<ResultPage<SaleBasedStockOperation>> BrowseSaleBasedStockOperations([Query] BrowseStockOperations query);

        [AllowAnyStatusCode]
        [Get("api/saleBasedStockOperations/{id}")]
        Task<SaleBasedStockOperation> GetSaleBasedStockOperation([Path] Guid id);
    }
}
