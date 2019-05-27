using System;

namespace Barista.Api.Authenticators
{
    public interface IAuthenticatorResult
    {
        Guid SubjectId { get; }
        Guid MeansId { get; }
    }
}
