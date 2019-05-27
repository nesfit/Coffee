using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Api.Commands.AuthenticationMeans;
using Barista.Api.Models.Identity;
using Barista.Api.Queries;
using Barista.Api.Services;
using Barista.Common;
using Barista.Common.OperationResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Barista.Api.Authenticators.Impl
{
    public class UserPasswordAuthenticator : PasswordAuthenticatorBase<AuthenticationMeansWithValue>, IUserPasswordAuthenticator
    {
        private readonly IBusPublisher _busPublisher;
        private readonly IUsersService _usersService;
        private readonly IIdentityService _identityService;
        private readonly ILogger<UserPasswordAuthenticator> _logger;
        
        public UserPasswordAuthenticator(IPasswordHasher<AuthenticationMeansWithValue> passwordHasher, IBusPublisher busPublisher, IUsersService usersService, IIdentityService identityService, ILogger<UserPasswordAuthenticator> logger) : base(passwordHasher)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task UpdateHashedPasswordAsync(AuthenticationMeansWithValue passwordContainer,string newHashedApiKey)
            => await UpdateHashedPasswordAsync(passwordContainer.Id, newHashedApiKey);

        protected async Task UpdateHashedPasswordAsync(Guid passwordMeansId, string newHashedApiKey)
            => await _busPublisher.SendRequest<UpdateAuthenticationMeansValue, IOperationResult>(new UpdateAuthenticationMeansValue(passwordMeansId, newHashedApiKey));

        public async Task<IAuthenticatorResult> AuthenticateAsync(string emailAddress, string providedPassword)
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));
            if (providedPassword == null) throw new ArgumentNullException(nameof(providedPassword));

            var usersPage = await _usersService.BrowseUsers(new BrowseUsers() { EmailAddressExact = emailAddress, ResultsPerPage = 1 });
            if (usersPage.TotalResults != 1)
            {
                if (usersPage.TotalResults > 1)
                    _logger.LogWarning("More than one user was returned to have exact email address of {emailAddress}", emailAddress);

                return null;
            }

            var user = usersPage.Items.Single();
            if (user.EmailAddress != emailAddress)
                return null;
            else if (!user.IsActive)
                return null;

            var passwordsPage = await _identityService.BrowseMeansAssignedToUser(user.Id, new BrowseAssignedMeans() { IsValid = true, Method = MeansMethod.Password, ResultsPerPage = 1 });
            if (passwordsPage.TotalResults != 1)
            {
                if (passwordsPage.TotalResults > 1)
                {
                    var userId = user.Id;
                    _logger.LogWarning("User with ID {userId} has more than 1 password", userId);
                }

                return null;
            }

            var password = passwordsPage.Items.Single();

            var assignments = await _identityService.BrowseAssignmentsToUser(new BrowseAssignmentsToUser {AssignedToUser = user.Id, OfAuthenticationMeans = password.Id});
            if (assignments.TotalResults != 1)
                return null;
            else if (assignments.Items[0].IsShared)
                return null;

            if (await Verify(password, password.Value, providedPassword))
                return new AuthenticatorResult(user.Id, password.Id);

            return null;
        }
    }
}
