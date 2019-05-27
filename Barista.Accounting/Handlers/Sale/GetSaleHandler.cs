using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Dto;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;

namespace Barista.Accounting.Handlers.Sale
{
    public class GetSaleHandler : IQueryHandler<GetSale, SaleDto>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IMapper _mapper;

        public GetSaleHandler(ISalesRepository salesRepository, IMapper mapper)
        {
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SaleDto> HandleAsync(GetSale query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            return _mapper.MapToWithNullPropagation<SaleDto>(await _salesRepository.GetAsync(query.Id));
        }
    }
}
