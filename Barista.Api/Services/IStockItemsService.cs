using System;
using System.Threading.Tasks;
using Barista.Api.Models.StockItems;
using Barista.Api.Queries;
using Barista.Common.Dto;
using RestEase;

namespace Barista.Api.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IStockItemsService
    {
        [AllowAnyStatusCode]
        [Get("api/stockItems")]
        Task<ResultPage<StockItem>> BrowseStockItems([Query] BrowseStockItems query);

        [AllowAnyStatusCode]
        [Get("api/stockItems/{id}")]
        Task<StockItem> GetStockItem([Path] Guid id);
    }
}
