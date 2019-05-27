using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.Assignment
{
    public class GetAssignmentHandler : IQueryHandler<GetAssignment, AssignmentDto>
    {
        private readonly IMapper _mapper;
        private readonly IAssignmentsRepository _repository;

        public GetAssignmentHandler(IAssignmentsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AssignmentDto> HandleAsync(GetAssignment query)
        {
            return _mapper.MapToWithNullPropagation<AssignmentDto>(await _repository.GetAsync(query.Id));
        }
    }
}
