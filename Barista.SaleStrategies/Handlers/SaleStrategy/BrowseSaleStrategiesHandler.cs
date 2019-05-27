using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.SaleStrategies.Dto;
using Barista.SaleStrategies.Queries;
using Barista.SaleStrategies.Repositories;

namespace Barista.SaleStrategies.Handlers.SaleStrategy
{
    public class BrowseSaleStrategiesHandler : IQueryHandler<BrowseSaleStrategies, IPagedResult<SaleStrategyDto>>
    {
        private readonly ISaleStrategyRepository _repository;
        private readonly IMapper _mapper;

        public BrowseSaleStrategiesHandler(ISaleStrategyRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<SaleStrategyDto>> HandleAsync(BrowseSaleStrategies query)
        {
            var results = await _repository.BrowseAsync(query);
            return _mapper.Map<IPagedResult<SaleStrategyDto>>(results);
        }
    }
}
