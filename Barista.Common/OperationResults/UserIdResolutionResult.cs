using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Barista.Common.OperationResults
{
    public class UserIdResolutionResult : OperationResult, IUserIdResolutionResult
    {
        public Guid? UserId { get; set; }
        public Guid? AssignmentId { get; }
        public bool? IsShared { get; }
        public KeyValuePair<TimeSpan, decimal>[] SpendingLimits { get; set; } = new KeyValuePair<TimeSpan, decimal>[0];
        ICollection<KeyValuePair<TimeSpan, decimal>> IUserIdResolutionResult.SpendingLimits => SpendingLimits;

        public UserIdResolutionResult(string errorCode, string errorMessage) : base(errorCode, errorMessage)
        {

        }

        public UserIdResolutionResult(Guid userId, Guid assignmentId, bool isShared, params KeyValuePair<TimeSpan, decimal>[] spendingLimits)
        {
            UserId = userId;
            SpendingLimits = spendingLimits;
            AssignmentId = assignmentId;
            IsShared = isShared;
        }

        [JsonConstructor]
        public UserIdResolutionResult(bool successful, DateTimeOffset created, string errorCode, string errorMessage, Guid userId, Guid assignmentId, bool isShared,
            KeyValuePair<TimeSpan, decimal>[] spendingLimits)
            : base(successful, errorCode, errorMessage, created)
        {
            UserId = userId;
            SpendingLimits = spendingLimits;
            AssignmentId = assignmentId;
            IsShared = isShared;
        }
    }
}
