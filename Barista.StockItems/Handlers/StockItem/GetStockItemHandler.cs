using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.StockItems.Dto;
using Barista.StockItems.Queries;
using Barista.StockItems.Repositories;

namespace Barista.StockItems.Handlers.StockItem
{
    public class GetStockItemHandler : IQueryHandler<GetStockItem, StockItemDto>
    {
        private readonly IStockItemRepository _repository;
        private readonly IMapper _mapper;

        public GetStockItemHandler(IStockItemRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<StockItemDto> HandleAsync(GetStockItem query)
        {
            return _mapper.MapToWithNullPropagation<StockItemDto>(await _repository.GetAsync(query.Id));
        }
    }
}
