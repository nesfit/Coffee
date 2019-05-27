using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.AccountingGroups.Dto;
using Barista.AccountingGroups.Queries;
using Barista.AccountingGroups.Repositories;
using Barista.Common;
using Barista.Contracts;

namespace Barista.AccountingGroups.Handlers.UserAuthorization
{
    public class BrowseUserAuthorizationsHandler : IQueryHandler<BrowseUserAuthorizations, IPagedResult<UserAuthorizationDto>>
    {
        private readonly IUserAuthorizationRepository _repository;
        private readonly IMapper _mapper;

        public BrowseUserAuthorizationsHandler(IUserAuthorizationRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<UserAuthorizationDto>> HandleAsync(BrowseUserAuthorizations query)
        {
            var results = await _repository.BrowseAsync(query);
            return _mapper.Map<IPagedResult<UserAuthorizationDto>>(results);
        }
    }
}
