using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.AssignmentToPointOfSale
{
    public class BrowseAssignmentsToPointOfSaleHandler : IQueryHandler<BrowseAssignmentsToPointOfSale, IPagedResult<AssignmentToPointOfSaleDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAssignmentsRepository _repository;

        public BrowseAssignmentsToPointOfSaleHandler(IAssignmentsRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<AssignmentToPointOfSaleDto>> HandleAsync(BrowseAssignmentsToPointOfSale query)
        {
            return _mapper.Map<IPagedResult<AssignmentToPointOfSaleDto>>(await _repository.BrowseAsync(query));
        }
    }
}
