using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.Assignment
{
    public class BrowseAssignmentsHandler : IQueryHandler<BrowseAssignments, IPagedResult<AssignmentDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAssignmentsRepository _repository;

        public BrowseAssignmentsHandler(IAssignmentsRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<AssignmentDto>> HandleAsync(BrowseAssignments query)
        {
            return _mapper.Map<IPagedResult<AssignmentDto>>(await _repository.BrowseAsync(query));
        }
    }
}
