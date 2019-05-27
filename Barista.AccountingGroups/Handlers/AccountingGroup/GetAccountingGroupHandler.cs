using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.AccountingGroups.Dto;
using Barista.AccountingGroups.Queries;
using Barista.AccountingGroups.Repositories;
using Barista.Common;

namespace Barista.AccountingGroups.Handlers.AccountingGroup
{
    public class GetAccountingGroupHandler : IQueryHandler<GetAccountingGroup, AccountingGroupDto>
    {
        private readonly IAccountingGroupRepository _repository;
        private readonly IMapper _mapper;

        public GetAccountingGroupHandler(IAccountingGroupRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<AccountingGroupDto> HandleAsync(GetAccountingGroup query)
        {
            var ag = await _repository.GetAsync(query.Id);
            return _mapper.MapToWithNullPropagation<AccountingGroupDto>(ag);
        }
    }
}
