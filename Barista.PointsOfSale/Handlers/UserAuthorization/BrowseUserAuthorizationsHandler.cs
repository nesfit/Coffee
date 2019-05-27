using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.PointsOfSale.Dto;
using Barista.PointsOfSale.Queries;
using Barista.PointsOfSale.Repositories;

namespace Barista.PointsOfSale.Handlers.UserAuthorization
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
