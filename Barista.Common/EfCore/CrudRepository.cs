using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Barista.Common.EfCore
{
    public abstract class CrudRepository<TContext, TEntity> : IBrowsableRepository<TEntity>
        where TEntity : class, IIdentifiable
        where TContext : DbContext
    {
        protected readonly TContext DbContext;
        protected readonly Func<TContext, DbSet<TEntity>> SetAccessorFunc;

        protected CrudRepository(TContext dbContext, Func<TContext, DbSet<TEntity>> dbSetAccessorFunc)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            SetAccessorFunc = dbSetAccessorFunc ?? throw new ArgumentNullException(nameof(dbSetAccessorFunc));
        }

        protected virtual IQueryable<TEntity> QueryableAccessorFunc() => SetAccessorFunc(DbContext);

        public virtual Task<TEntity> GetAsync(Guid id) => QueryableAccessorFunc().SingleOrDefaultAsync(e => e.Id == id);

        public Task AddAsync(TEntity entity) => SetAccessorFunc(DbContext).AddAsync(entity);

        public Task UpdateAsync(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TEntity entity)
        {
            SetAccessorFunc(DbContext).Remove(entity);
            return Task.CompletedTask;
        }

        public virtual async Task<IPagedResult<TEntity>> BrowseAsync(IPagedQueryImpl<TEntity> pagedQuery)
        {
            return await QueryableAccessorFunc().PaginateAsync(pagedQuery);
        }

        public async Task SaveChanges()
        {
            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e) when (e.InnerException.Message.StartsWith("Duplicate entry"))
            {
                throw new EntityAlreadyExistsException(e);
            }
        }
    }
}
