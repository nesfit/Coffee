using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.SpendingLimit
{
    public class GetSpendingLimitHandler : SpendingLimitHandlerBase, IQueryHandler<GetSpendingLimit, SpendingLimitDto>
    {
        private readonly IAssignmentsRepository _assignmentsRepository;
        private readonly IMapper _mapper;

        public GetSpendingLimitHandler(IAssignmentsRepository assignmentsRepository, IMapper mapper)
        {
            _assignmentsRepository = assignmentsRepository ?? throw new ArgumentNullException(nameof(assignmentsRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SpendingLimitDto> HandleAsync(GetSpendingLimit query)
        {
            var assignmentToUser = await GetAssignmentToUser(_assignmentsRepository, query.ParentAssignmentToUserId);
            if (assignmentToUser is null)
                throw new BaristaException("assignment_to_user_not_found", $"Could not find assignment to user with ID '{query.ParentAssignmentToUserId}'.");

            var spendingLimit = assignmentToUser.SpendingLimits.FirstOrDefault(sl => sl.Id == query.SpendingLimitId);
            return _mapper.MapToWithNullPropagation<SpendingLimitDto>(spendingLimit);
        }
    }
}
