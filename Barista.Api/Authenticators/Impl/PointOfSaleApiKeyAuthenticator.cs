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
    public class PointOfSaleApiKeyAuthenticator : PasswordAuthenticatorBase<AuthenticationMeansWithValue>, IPointOfSaleApiKeyAuthenticator
    {
        private readonly IBusPublisher _busPublisher;
        private readonly IPointsOfSaleService _posService;
        private readonly IIdentityService _identityService;
        
        public PointOfSaleApiKeyAuthenticator(IPasswordHasher<AuthenticationMeansWithValue> passwordHasher, IBusPublisher busPublisher, IPointsOfSaleService posService, IIdentityService identityService) : base(passwordHasher)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _posService = posService ?? throw new ArgumentNullException(nameof(posService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        protected override async Task UpdateHashedPasswordAsync(AuthenticationMeansWithValue passwordContainer, string newHashedApiKey)
            => await _busPublisher.SendRequest<UpdateAuthenticationMeansValue, IOperationResult>(new UpdateAuthenticationMeansValue(passwordContainer.Id, newHashedApiKey));

        public async Task<IAuthenticatorResult> AuthenticateAsync(Guid posId, string providedApiKey)
        {
            if (providedApiKey == null)
                throw new ArgumentNullException(nameof(providedApiKey));

            var pointOfSale = await _posService.GetPointOfSale(posId);
            if (pointOfSale is null)
                return null;
            
            var query = new BrowseAssignedMeans() {ResultsPerPage = 100, IsValid = true, Method = MeansMethod.ApiKey};
            ResultPage<AuthenticationMeansWithValue> resultPage;

            do
            {
                resultPage = await _identityService.BrowseMeansAssignedToPointOfSale(posId, query);
                query.CurrentPage++;

                foreach (var password in resultPage.Items)
                {
                    if (await Verify(password, password.Value, providedApiKey))
                        return new AuthenticatorResult(posId, password.Id);
                }
            } while (resultPage.Items.Any() && resultPage.CurrentPage < resultPage.TotalPages);

            return null;
        }
    }
}
