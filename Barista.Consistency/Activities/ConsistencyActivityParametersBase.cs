using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Barista.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Barista.Consistency.Activities
{
    public class ConsistencyActivityParametersBase : IConsistencyActivityParameters
    {
        public ISourceEventData SourceEventData { get; set; }
        public int RerunRequiredTimes { get; set; }

        public IEvent SourceEvent
        {
            set => SourceEventData = new SourceEventData(
                value.GetType().GetInterfaces().Single(i => i.GetInterfaces().Contains(typeof(IEvent))).AssemblyQualifiedName,
                JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings() { ContractResolver = new EventPropertiesResolver()}),
                DateTimeOffset.UtcNow
            );
        }
    }

    public class EventPropertiesResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            if (objectType.GetInterfaces().SingleOrDefault(i => i.GetInterfaces().Contains(typeof(IEvent))) is Type eventInterface)
                return base.CreateContract(eventInterface);

            return base.CreateContract(objectType);
        }
    }
}
