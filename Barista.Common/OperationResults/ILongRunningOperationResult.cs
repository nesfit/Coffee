using System;

namespace Barista.Common.OperationResults
{
    public interface ILongRunningOperationResult : IOperationResult
    {
        Guid? LongRunningOperationId { get; }
    }
}
