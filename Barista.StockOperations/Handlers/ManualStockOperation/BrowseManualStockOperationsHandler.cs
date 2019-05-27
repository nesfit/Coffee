using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Queries;
using Barista.StockOperations.Repositories;

namespace Barista.StockOperations.Handlers.ManualStockOperation
{
    public class BrowseManualStockOperationsHandler : IQueryHandler<BrowseManualStockOperations, IPagedResult<ManualStockOperationDto>>
    {
        private readonly IStockOperationRepository _repository;
        private readonly IMapper _mapper;

        public BrowseManualStockOperationsHandler(IStockOperationRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<ManualStockOperationDto>> HandleAsync(BrowseManualStockOperations query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var results = await _repository.BrowseAsync(query);
            return _mapper.Map<IPagedResult<ManualStockOperationDto>>(results);
        }
    }
}
