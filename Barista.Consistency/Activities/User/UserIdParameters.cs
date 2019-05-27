using System;

namespace Barista.Consistency.Activities.User
{
    public class UserIdParameters : ConsistencyActivityParametersBase
    {
        public Guid UserId { get; set; }
    }
}
