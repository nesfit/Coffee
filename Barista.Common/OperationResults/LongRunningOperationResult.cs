using System;
using Newtonsoft.Json;

namespace Barista.Common.OperationResults
{
    public class LongRunningOperationResult : OperationResult, ILongRunningOperationResult
    {
        public Guid? LongRunningOperationId { get; }

        public LongRunningOperationResult(string errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }

        public LongRunningOperationResult(Guid longRunningOperationId) : base()
        {
            LongRunningOperationId = longRunningOperationId;
        }

        [JsonConstructor]
        public LongRunningOperationResult(Guid? longRunningOperationId, Guid? id, bool successful, string errorCode, string errorMessage, DateTimeOffset created) : base(successful, errorCode, errorMessage, created)
        {
            LongRunningOperationId = longRunningOperationId;
        }
    }
}
