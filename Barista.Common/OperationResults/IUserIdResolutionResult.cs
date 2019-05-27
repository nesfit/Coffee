using System;
using System.Collections.Generic;

namespace Barista.Common.OperationResults
{
    public interface IUserIdResolutionResult : IOperationResult
    {
        Guid? UserId { get; }
        Guid? AssignmentId { get; }
        bool? IsShared { get; }
        ICollection<KeyValuePair<TimeSpan, decimal>> SpendingLimits { get; }
    }
}
