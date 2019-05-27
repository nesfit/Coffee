using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Queries;
using Barista.StockOperations.Repositories;

namespace Barista.StockOperations.Handlers.StockOperation
{
    public class GetStockOperationHandler : IQueryHandler<GetStockOperation, StockOperationDto>
    {
        private readonly IStockOperationRepository _repository;
        private readonly IMapper _mapper;

        public GetStockOperationHandler(IStockOperationRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<StockOperationDto> HandleAsync(GetStockOperation query)
        {
            var stockOp = await _repository.GetAsync(query.Id);
            return _mapper.MapToWithNullPropagation<StockOperationDto>(stockOp);
        }
    }
}
