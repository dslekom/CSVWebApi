using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.DAL.Interfaces;

namespace WebApi.DAL.Repositories
{
    /// <summary>
    /// Базовая реализация репозитория
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public Repository(DbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.dbSet = context.Set<TEntity>();
        }

        public virtual async Task CreateAsync(TEntity item)
        {
            await dbSet.AddAsync(item);
        }

        public virtual async Task CreateRangeAsync(IEnumerable<TEntity> items)
        {
            await dbSet.AddRangeAsync(items);
        }

        public virtual void Delete(object id)
        {
            var item = dbSet.Find(id);

            if (item != null)
            {
                context.Entry(item).State = EntityState.Deleted;
            }
        }

        public virtual void DeleteRange(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
            {
                context.Entry(item).State = EntityState.Deleted;
            }
        }

        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public virtual async Task<TEntity> GetAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return dbSet;
        }

        public virtual void Update(TEntity item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
