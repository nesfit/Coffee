using System;

namespace Barista.Api.Models.Operation
{
    public class Operation
    {
        public Guid Id { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsFaulted { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
