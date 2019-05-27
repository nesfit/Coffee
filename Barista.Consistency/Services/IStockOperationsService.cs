using System.Threading.Tasks;
using Barista.Common.Dto;
using Barista.Consistency.Models;
using Barista.Consistency.Queries;
using RestEase;

namespace Barista.Consistency.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IStockOperationsService
    {
        [AllowAnyStatusCode]
        [Get("api/manualStockOperations")]
        Task<ResultPage<StockOperation>> BrowseManualStockOperations([Query] BrowseManualStockOperations query);

        [AllowAnyStatusCode]
        [Get("api/saleBasedStockOperations")]
        Task<ResultPage<StockOperation>> BrowseSaleBasedStockOperations([Query] BrowseSaleBasedStockOperations query);
    }
}
