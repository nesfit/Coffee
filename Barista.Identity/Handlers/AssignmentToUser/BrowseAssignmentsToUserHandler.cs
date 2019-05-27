using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.AssignmentToUser
{
    public class BrowseAssignmentsToUserHandler : IQueryHandler<BrowseAssignmentsToUser, IPagedResult<AssignmentToUserDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAssignmentsRepository _repository;

        public BrowseAssignmentsToUserHandler(IAssignmentsRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<AssignmentToUserDto>> HandleAsync(BrowseAssignmentsToUser query)
        {
            return _mapper.Map<IPagedResult<AssignmentToUserDto>>(await _repository.BrowseAsync(query));
        }
    }
}
