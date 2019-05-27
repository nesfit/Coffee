using System;

namespace Barista.Common.OperationResults
{
    public interface IIdentifierResult : IOperationResult
    {
        Guid? Id { get; }
    }
}
