using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Identity.Domain;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;

namespace Barista.Identity.Verifiers
{
    public class AssignmentExclusivityVerifier : IAssignmentExclusivityVerifier
    {
        private readonly IAssignmentsRepository _assignmentsRepository;

        public AssignmentExclusivityVerifier(IAssignmentsRepository assignmentsRepository)
        {
            _assignmentsRepository = assignmentsRepository ?? throw new ArgumentNullException(nameof(assignmentsRepository));
        }

        public async Task VerifyAssignmentExclusivity(Guid meansId)
        {
            var query = new BrowseAssignments
            {
                OfAuthenticationMeans = meansId,
                SortBy = new[] {$"{nameof(Assignment.ValidSince)} asc", $"{nameof(Assignment.ValidUntil)} asc"}
            };

            Assignment currentlyValid = null;
            int recordsVisited;

            do
            {
                recordsVisited = 0;
                var resultsPage = await _assignmentsRepository.BrowseAsync(query);

                foreach (var assignment in resultsPage.Items)
                {
                    if (currentlyValid != null)
                    {
                        if (currentlyValid.ValidUntil == null)
                            throw new BaristaException(
                                "invalid_means_assignment_status",

                                $"The means with ID '{meansId}' cannot have more than one active assignment. " +
                                $"The assignment with ID '{currentlyValid.Id}' becomes permanently valid at '{currentlyValid.ValidSince}', " +
                                $"and the next assignment with ID '{assignment.Id}' becomes valid at '{assignment.ValidSince}'."
                            );
                        else if (currentlyValid.ValidUntil > assignment.ValidSince)
                        {
                            throw new BaristaException(
                                "invalid_means_assignment_status",

                                $"The means with ID '{meansId}' cannot have more than one active assignment. " +
                                $"The assignment with ID '{currentlyValid.Id}' is valid from '{currentlyValid.ValidSince}' until '{currentlyValid.ValidUntil}'" +
                                $"and the next assignment with ID '{assignment.Id}' becomes valid at '{assignment.ValidSince}'."
                            );
                        }
                    }

                    currentlyValid = assignment;
                    recordsVisited++;
                }

                query.CurrentPage++;
            } while (recordsVisited > 0);
        }
    }
}
