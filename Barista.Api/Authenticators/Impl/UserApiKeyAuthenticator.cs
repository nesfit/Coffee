using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Api.Commands.AuthenticationMeans;
using Barista.Api.Models.Identity;
using Barista.Api.Queries;
using Barista.Api.Services;
using Barista.Common;
using Barista.Common.Dto;
using Barista.Common.OperationResults;
using Microsoft.AspNetCore.Identity;

namespace Barista.Api.Authenticators.Impl
{
    public class UserApiKeyAuthenticator : PasswordAuthenticatorBase<AuthenticationMeansWithValue>, IUserApiKeyAuthenticator
    {
        private readonly IBusPublisher _busPublisher;
        private readonly IUsersService _usersService;
        private readonly IIdentityService _identityService;
        
        public UserApiKeyAuthenticator(IPasswordHasher<AuthenticationMeansWithValue> passwordHasher, IBusPublisher busPublisher, IUsersService usersService, IIdentityService identityService) : base(passwordHasher)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        protected override async Task UpdateHashedPasswordAsync(AuthenticationMeansWithValue passwordContainer, string newHashedApiKey)
            => await _busPublisher.SendRequest<UpdateAuthenticationMeansValue, IOperationResult>(new UpdateAuthenticationMeansValue(passwordContainer.Id, newHashedApiKey));

        public async Task<IAuthenticatorResult> AuthenticateAsync(Guid userId, string providedApiKey)
        {
            if (providedApiKey == null)
                throw new ArgumentNullException(nameof(providedApiKey));

            var user = await _usersService.GetUser(userId);
            if (user is null)
                return null;

            if (!user.IsActive)
                return null;
            
            var query = new BrowseAssignedMeans() {ResultsPerPage = 100, IsValid = true, Method = MeansMethod.ApiKey};
            ResultPage<AuthenticationMeansWithValue> resultPage;

            do
            {
                resultPage = await _identityService.BrowseMeansAssignedToUser(userId, query);
                query.CurrentPage++;

                foreach (var password in resultPage.Items)
                {
                    if (await Verify(password, password.Value, providedApiKey))
                        return new AuthenticatorResult(userId, password.Id);
                }
            } while (resultPage.Items.Any() && resultPage.CurrentPage < resultPage.TotalPages);

            return null;
        }
    }
}
