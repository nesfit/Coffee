using System;

namespace Barista.Api.Authenticators.Impl
{
    public class AuthenticatorResult : IAuthenticatorResult
    {
        public AuthenticatorResult(Guid subjectId, Guid meansId)
        {
            SubjectId = subjectId;
            MeansId = meansId;
        }

        public Guid SubjectId { get; }
        public Guid MeansId { get; }
    }
}
