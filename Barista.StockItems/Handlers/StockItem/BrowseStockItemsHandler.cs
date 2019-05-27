using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.StockItems.Dto;
using Barista.StockItems.Queries;
using Barista.StockItems.Repositories;

namespace Barista.StockItems.Handlers.StockItem
{
    public class BrowseStockItemsHandler : IQueryHandler<BrowseStockItems, IPagedResult<StockItemDto>>
    {
        private readonly IStockItemRepository _repository;
        private readonly IMapper _mapper;

        public BrowseStockItemsHandler(IStockItemRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<StockItemDto>> HandleAsync(BrowseStockItems query)
        {
            var result = await _repository.BrowseAsync(query);
            return _mapper.Map<IPagedResult<StockItemDto>>(result);
        }
    }
}
