using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Users.Dto;
using Barista.Users.Queries;
using Barista.Users.Repositories;

namespace Barista.Users.Handlers.User
{
    public class GetUserHandler : IQueryHandler<GetUser, UserDto>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public GetUserHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDto> HandleAsync(GetUser query)
        {
            var user = await _repository.GetAsync(query.Id);
            return _mapper.MapToWithNullPropagation<UserDto>(user);
        }
    }
}
