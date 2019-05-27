using System;
using System.Threading.Tasks;

namespace Barista.Identity.Verifiers
{
    public interface IAssignmentExclusivityVerifier
    {
        Task VerifyAssignmentExclusivity(Guid meansId);
    }
}
