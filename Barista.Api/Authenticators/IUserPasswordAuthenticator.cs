using System;
using System.Threading.Tasks;

namespace Barista.Api.Authenticators
{
    public interface IUserPasswordAuthenticator
    {
        Task<IAuthenticatorResult> AuthenticateAsync(string emailAddress, string providedPassword);
    }
}
