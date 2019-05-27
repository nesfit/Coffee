using System;

namespace Barista.Operations.Dto
{
    public class OperationDto
    {
        public Guid Id { get; }
        public bool IsCompleted { get; }
        public bool IsFaulted { get; }
        public TimeSpan? Duration { get; }

        public OperationDto(Guid id, bool isCompleted, bool isFaulted, TimeSpan? duration)
        {
            Id = id;
            IsCompleted = isCompleted;
            IsFaulted = isFaulted;
            Duration = duration;
        }
    }
}
