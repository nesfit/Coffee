using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Dto;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;

namespace Barista.Accounting.Handlers.SaleStateChange
{
    public class GetSaleStateChangeHandler : IQueryHandler<GetSaleStateChange, SaleStateChangeDto>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IMapper _mapper;

        public GetSaleStateChangeHandler(ISalesRepository salesRepository, IMapper mapper)
        {
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SaleStateChangeDto> HandleAsync(GetSaleStateChange query)
        {
            var sale = await _salesRepository.GetAsync(query.ParentSaleId);
            if (sale is null)
                throw new BaristaException("sale_not_found", $"Could not find sale with ID '{query.ParentSaleId}'");

            return _mapper.MapToWithNullPropagation<SaleStateChangeDto>(sale.StateChanges.FirstOrDefault(chg => chg.Id == query.SaleStateChangeId));
        }
    }
}
