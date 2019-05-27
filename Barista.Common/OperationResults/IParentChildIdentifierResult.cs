using System;

namespace Barista.Common.OperationResults
{
    public interface IParentChildIdentifierResult : IOperationResult
    {
        Guid? ParentId { get; }
        Guid? ChildId { get; }
    }
}
