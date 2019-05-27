using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.PointsOfSale.Dto;
using Barista.PointsOfSale.Queries;
using Barista.PointsOfSale.Repositories;

namespace Barista.PointsOfSale.Handlers.UserAuthorization
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
            return _mapper.MapToWithNullPropagation<UserAuthorizationDto>(await _repository.GetAsync(query.PointOfSaleId, query.UserId));
        }
    }
}
