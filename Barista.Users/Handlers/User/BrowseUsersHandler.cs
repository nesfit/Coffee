using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.Users.Dto;
using Barista.Users.Queries;
using Barista.Users.Repositories;

namespace Barista.Users.Handlers.User
{
    public class BrowseUsersHandler : IQueryHandler<BrowseUsers, IPagedResult<UserDto>>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public BrowseUsersHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<UserDto>> HandleAsync(BrowseUsers query)
        {
            var results = await _repository.BrowseAsync(query);
            return _mapper.Map<IPagedResult<UserDto>>(results);
        }
    }
}
