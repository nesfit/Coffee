using System;
using Barista.Common.EfCore;
using Barista.Consistency.Domain;
using Microsoft.EntityFrameworkCore;

namespace Barista.Consistency.Repositories
{
    public class ScheduledEventRepository : CrudRepository<ConsistencyDbContext, ScheduledEvent>, IScheduledEventRepository
    {
        public ScheduledEventRepository(ConsistencyDbContext dbContext) : base(dbContext, dbc => dbc.ScheduledEvents)
        {
        }
    }
}
