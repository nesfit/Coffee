using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.SaleStrategies.Dto;
using Barista.SaleStrategies.Queries;
using Barista.SaleStrategies.Repositories;

namespace Barista.SaleStrategies.Handlers.SaleStrategy
{
    public class GetSaleStrategyHandler : IQueryHandler<GetSaleStrategy, SaleStrategyDto>
    {
        private readonly ISaleStrategyRepository _repository;
        private readonly IMapper _mapper;

        public GetSaleStrategyHandler(ISaleStrategyRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SaleStrategyDto> HandleAsync(GetSaleStrategy query)
        {
            return _mapper.MapToWithNullPropagation<SaleStrategyDto>(await _repository.GetSaleStrategy(query.Id));
        }
    }

}
