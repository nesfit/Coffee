using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;

namespace Barista.AccountingGroups.Verifiers
{
    public interface IAccountingGroupVerifier : IExistenceVerifier<Guid>
    {
    }
}
