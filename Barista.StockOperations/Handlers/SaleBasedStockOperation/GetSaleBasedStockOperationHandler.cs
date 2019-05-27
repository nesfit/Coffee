using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Queries;
using Barista.StockOperations.Repositories;

namespace Barista.StockOperations.Handlers.SaleBasedStockOperation
{
    public class GetSaleBasedStockOperationHandler : IQueryHandler<GetSaleBasedStockOperation, SaleBasedStockOperationDto>
    {
        private readonly IStockOperationRepository _repository;
        private readonly IMapper _mapper;

        public GetSaleBasedStockOperationHandler(IStockOperationRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SaleBasedStockOperationDto> HandleAsync(GetSaleBasedStockOperation query)
        {
            var stockOp = await _repository.GetAsync(query.Id);
            return _mapper.MapToWithNullPropagation<SaleBasedStockOperationDto>(stockOp);
        }
    }
}
