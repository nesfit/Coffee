using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.Dispatchers;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AuthenticationMeans;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Services;

namespace Barista.Identity.Handlers.AuthenticationMeans
{
    // TODO: unit test
    public class ResolveAuthenticationMeansUserIdHandler : ICommandHandler<IResolveAuthenticationMeansUserId, IUserIdResolutionResult>
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IUsersService _usersService;

        public ResolveAuthenticationMeansUserIdHandler(IQueryDispatcher queryDispatcher, IUsersService usersService)
        {
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public async Task<IUserIdResolutionResult> HandleAsync(IResolveAuthenticationMeansUserId command, ICorrelationContext correlationContext)
        {
            var authenticationMeans = await _queryDispatcher.QueryAsync(new GetAuthenticationMeans(command.AuthenticationMeansId));
            if (authenticationMeans is null)
                return new UserIdResolutionResult("authentication_means_not_found", $"Could not find authentication means with ID '{command.AuthenticationMeansId}'");

            if (!authenticationMeans.IsValid)
                return new UserIdResolutionResult("authentication_means_not_valid", $"The authentication means with ID '{command.AuthenticationMeansId}' is not valid");

            AssignmentToUserDto validUserAssignment = null;

            var now = DateTimeOffset.UtcNow;

            await _queryDispatcher.QueryAllPages( // todo
                new BrowseAssignmentsToUser() {OfAuthenticationMeans = authenticationMeans.Id},
                assignment =>
                {
                    if (assignment.ValidSince > now)
                        return false;

                    if (assignment.ValidUntil < now)
                        return false;

                    if (assignment.IsShared && command.ExcludeSharedAuthenticationMeans)
                        return false;

                    validUserAssignment = assignment;
                    return true;
                }
            );

            if (validUserAssignment is null)
                return new UserIdResolutionResult("authentication_means_not_assigned", $"The authentication means with ID '{command.AuthenticationMeansId}' is not assigned to any user");

            var user = await _usersService.GetUser(validUserAssignment.AssignedToUserId);
            if (user is null)
                return new UserIdResolutionResult("authentication_means_assignment_not_valid", $"The authentication means with ID '{command.AuthenticationMeansId}' is assigned to user with ID '{validUserAssignment.AssignedToUserId}' which was not found.");
            else if (!user.IsActive)
                return new UserIdResolutionResult("authentication_means_assignment_not_valid", $"The authentication means with ID '{command.AuthenticationMeansId}' is assigned to user with ID '{validUserAssignment.AssignedToUserId}' which is an inactive account.");

            return new UserIdResolutionResult(
                validUserAssignment.AssignedToUserId,
                validUserAssignment.Id,
                validUserAssignment.IsShared,
                validUserAssignment.SpendingLimits.Select(spl => new KeyValuePair<TimeSpan, decimal>(spl.Interval, spl.Value)).ToArray()
            );
        }
    }
}
