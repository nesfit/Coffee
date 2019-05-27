using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.AssignmentToUser
{
    public class GetAssignmentToUserHandler : IQueryHandler<GetAssignmentToUser, AssignmentToUserDto>
    {
        private readonly IMapper _mapper;
        private readonly IAssignmentsRepository _repository;

        public GetAssignmentToUserHandler(IAssignmentsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AssignmentToUserDto> HandleAsync(GetAssignmentToUser query)
        {
            return _mapper.MapToWithNullPropagation<AssignmentToUserDto>((await _repository.GetAsync(query.Id)) as Domain.AssignmentToUser);
        }
    }
}
