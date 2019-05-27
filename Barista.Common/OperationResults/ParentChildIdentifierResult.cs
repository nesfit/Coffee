using System;
using Newtonsoft.Json;

namespace Barista.Common.OperationResults
{
    public class ParentChildIdentifierResult : OperationResult, IParentChildIdentifierResult
    {
        public Guid? ParentId { get; }
        public Guid? ChildId { get; }

        public ParentChildIdentifierResult(Guid parentId, Guid childId) : base()
        {
            ParentId = parentId;
            ChildId = childId;
        }

        public ParentChildIdentifierResult(string errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }

        [JsonConstructor]
        public ParentChildIdentifierResult(string errorCode, string errorMessage, bool successful, DateTimeOffset created, Guid parentId, Guid childId) : base(successful, errorCode, errorMessage, created)
        {
            ParentId = parentId;
            ChildId = childId;
        }
    }
}
