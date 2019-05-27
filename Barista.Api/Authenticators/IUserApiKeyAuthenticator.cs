using System;
using System.Threading.Tasks;

namespace Barista.Api.Authenticators
{
    public interface IUserApiKeyAuthenticator
    {
        Task<IAuthenticatorResult> AuthenticateAsync(Guid userId, string providedApiKey);
    }
}
