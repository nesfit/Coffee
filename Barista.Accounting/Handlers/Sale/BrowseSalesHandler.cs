using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Dto;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;
using Barista.Contracts;

namespace Barista.Accounting.Handlers.Sale
{
    public class BrowseSalesHandler : IQueryHandler<BrowseSales, IPagedResult<SaleDto>>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IMapper _mapper;

        public BrowseSalesHandler(ISalesRepository salesRepository, IMapper mapper)
        {
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<SaleDto>> HandleAsync(BrowseSales query)
            => _mapper.Map<IPagedResult<SaleDto>>(await _salesRepository.BrowseAsync(query));
    }
}
