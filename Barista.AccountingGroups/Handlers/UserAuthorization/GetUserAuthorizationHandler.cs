using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.AccountingGroups.Dto;
using Barista.AccountingGroups.Queries;
using Barista.AccountingGroups.Repositories;
using Barista.Common;

namespace Barista.AccountingGroups.Handlers.UserAuthorization
{
    public class GetUserAuthorizationHandler : IQueryHandler<GetUserAuthorization, UserAuthorizationDto>
    {
        private readonly IUserAuthorizationRepository _repository;
        private readonly IMapper _mapper;

        public GetUserAuthorizationHandler(IUserAuthorizationRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserAuthorizationDto> HandleAsync(GetUserAuthorization query)
        {
            var userAuth = await _repository.GetAsync(query.AccountingGroupId, query.UserId);
            return _mapper.Map<UserAuthorizationDto>(userAuth);
        }
    }
}
