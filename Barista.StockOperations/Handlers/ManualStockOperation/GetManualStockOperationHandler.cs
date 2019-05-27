using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Queries;
using Barista.StockOperations.Repositories;

namespace Barista.StockOperations.Handlers.ManualStockOperation
{
    public class GetManualStockOperationHandler : IQueryHandler<GetManualStockOperation, ManualStockOperationDto>
    {
        private readonly IStockOperationRepository _repository;
        private readonly IMapper _mapper;

        public GetManualStockOperationHandler(IStockOperationRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ManualStockOperationDto> HandleAsync(GetManualStockOperation query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var stockOp = await _repository.GetAsync(query.Id);
            return _mapper.MapToWithNullPropagation<ManualStockOperationDto>(stockOp);
        }
    }
}
