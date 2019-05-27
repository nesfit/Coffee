using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.AccountingGroups.Dto;
using Barista.AccountingGroups.Queries;
using Barista.AccountingGroups.Repositories;
using Barista.Common;
using Barista.Contracts;

namespace Barista.AccountingGroups.Handlers.AccountingGroup
{
    public class BrowseAccountingGroupsHandler : IQueryHandler<BrowseAccountingGroups, IPagedResult<AccountingGroupDto>>
    {
        private readonly IAccountingGroupRepository _repository;
        private readonly IMapper _mapper;

        public BrowseAccountingGroupsHandler(IAccountingGroupRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<AccountingGroupDto>> HandleAsync(BrowseAccountingGroups query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            var results = await _repository.BrowseAsync(query);
            return _mapper.Map<IPagedResult<AccountingGroupDto>>(results);
        }
    }
}
