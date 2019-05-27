using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barista.Consistency.Activities
{
    public class ConsistencyRemediationVariables : IConsistencyRemediationVariables
    {
        public int RerunRequiredTimes { get; set; }
    }
}
