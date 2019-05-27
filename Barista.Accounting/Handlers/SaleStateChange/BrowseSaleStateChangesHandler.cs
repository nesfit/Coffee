using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Dto;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;
using Barista.Contracts;

namespace Barista.Accounting.Handlers.SaleStateChange
{
    public class BrowseSaleStateChangesHandler : IQueryHandler<BrowseSaleStateChanges, IPagedResult<SaleStateChangeDto>>
    {
        private readonly IClientsidePaginator<Domain.SaleStateChange> _paginator;
        private readonly ISalesRepository _salesRepository;
        private readonly IMapper _mapper;

        public BrowseSaleStateChangesHandler(ISalesRepository salesRepository, IMapper mapper, IClientsidePaginator<Domain.SaleStateChange> paginator)
        {
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _paginator = paginator ?? throw new ArgumentNullException(nameof(paginator));
        }

        public async Task<IPagedResult<SaleStateChangeDto>> HandleAsync(BrowseSaleStateChanges query)
        {
            var sale = await _salesRepository.GetAsync(query.ParentSaleId);
            if (sale is null)
                throw new BaristaException("sale_not_found", $"Could not find sale with ID {query.ParentSaleId}");

            var results = await _paginator.PaginateAsync(sale.StateChanges, query);
            return _mapper.Map<IPagedResult<SaleStateChangeDto>>(results);
        }
    }
}
