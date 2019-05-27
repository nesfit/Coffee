using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.SpendingLimit
{
    public class BrowseSpendingLimitsHandler : SpendingLimitHandlerBase, IQueryHandler<BrowseSpendingLimits, IPagedResult<SpendingLimitDto>>
    {
        private readonly IAssignmentsRepository _repository;
        private readonly IClientsidePaginator<Domain.SpendingLimit> _paginator;
        private readonly IMapper _mapper;

        public BrowseSpendingLimitsHandler(IAssignmentsRepository repository, IClientsidePaginator<Domain.SpendingLimit> paginator, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _paginator = paginator ?? throw new ArgumentNullException(nameof(paginator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<SpendingLimitDto>> HandleAsync(BrowseSpendingLimits query)
        {
            var assignmentToUser = await GetAssignmentToUser(_repository, query.ParentAssignmentToUserId);
            var pagedResult = await _paginator.PaginateAsync(assignmentToUser.SpendingLimits, query);
            return _mapper.Map<IPagedResult<SpendingLimitDto>>(pagedResult);
        }
    }
}
