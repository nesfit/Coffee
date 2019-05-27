using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Common;
using Barista.StockOperations.Services;

namespace Barista.StockOperations.Verifiers
{
    public class StockItemVerifier : ExistenceVerifierBase<Guid>, IStockItemVerifier
    {
        private readonly IStockItemsService _service;

        public StockItemVerifier(IStockItemsService service)
            : base("stock_item", "stock item")
        {
            _service = service;
        }

        protected override async Task<HttpResponseMessage> MakeRequest(Guid id)
            => await _service.StatStockItem(id);
    }
}
