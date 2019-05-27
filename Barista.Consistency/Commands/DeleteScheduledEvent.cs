using System;
using Barista.Contracts;

namespace Barista.Consistency.Commands
{
    public class DeleteScheduledEvent : ICommand
    {
        public DeleteScheduledEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
