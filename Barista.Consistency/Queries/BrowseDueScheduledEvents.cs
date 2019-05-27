using System;
using System.Linq;
using Barista.Common;
using Barista.Consistency.Domain;

namespace Barista.Consistency.Queries
{
    public class BrowseDueScheduledEvents : IPagedQueryImpl<ScheduledEvent>
    {
        public IQueryable<ScheduledEvent> Apply(IQueryable<ScheduledEvent> q)
            => q.ApplySort(SortBy).Where(scheduledEvent => scheduledEvent.ScheduledFor < DateTimeOffset.UtcNow);

        public int CurrentPage { get; } = 1;
        public int ResultsPerPage { get; } = 10;
        public string[] SortBy { get; } = new string[0];
    }
}
