using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Contracts;
using Barista.Identity.Domain;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.SpendingLimit
{
    public abstract class SpendingLimitHandlerBase : ICommand
    {
        protected async Task<Domain.AssignmentToUser> GetAssignmentToUser(IAssignmentsRepository repository, Guid assignmentId)
        {
            var assignment = await repository.GetAsync(assignmentId);
            if (assignment is null)
                throw new BaristaException("assignment_not_found", $"Assignment with ID '{assignmentId}' was not found");
            if (!(assignment is Domain.AssignmentToUser assignmentToUser))
                throw new BaristaException("invalid_assignment_type", $"Assignment with ID '{assignmentId}' does not support spending limits");

            return assignmentToUser;
        }
    }
}
