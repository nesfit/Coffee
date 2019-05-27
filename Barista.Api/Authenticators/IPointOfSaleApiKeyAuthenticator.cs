using System;
using System.Threading.Tasks;

namespace Barista.Api.Authenticators
{
    public interface IPointOfSaleApiKeyAuthenticator
    {
        Task<IAuthenticatorResult> AuthenticateAsync(Guid posId, string providedApiKey);
    }
}
