using System;

namespace Barista.Common.OperationResults
{
    public interface IOperationResult
    {
        bool Successful { get; }
        string ErrorCode { get; }
        string ErrorMessage { get; }
        DateTimeOffset Created { get; }
    }
}
