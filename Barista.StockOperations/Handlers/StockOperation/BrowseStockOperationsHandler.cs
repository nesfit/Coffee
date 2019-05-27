using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Queries;
using Barista.StockOperations.Repositories;

namespace Barista.StockOperations.Handlers.StockOperation
{
    public class BrowseStockOperationsHandler : IQueryHandler<BrowseStockOperations, IPagedResult<StockOperationDto>>
    {
        private readonly IStockOperationRepository _repository;
        private readonly IMapper _mapper;

        public BrowseStockOperationsHandler(IStockOperationRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<StockOperationDto>> HandleAsync(BrowseStockOperations query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var results = await _repository.BrowseAsync(query);
            return _mapper.Map<IPagedResult<StockOperationDto>>(results);
        }
    }
}
