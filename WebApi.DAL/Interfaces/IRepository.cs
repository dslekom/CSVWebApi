using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApi.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetAsync(object id);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task CreateAsync(TEntity item);
        Task CreateRangeAsync(IEnumerable<TEntity> items);
        void Update(TEntity item);
        void Delete(object id);
        void DeleteRange(IEnumerable<TEntity> items);
    }
}
