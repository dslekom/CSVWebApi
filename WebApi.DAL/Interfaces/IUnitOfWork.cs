using System;
using System.Threading.Tasks;

namespace WebApi.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        Task<int> SaveAsync();
    }
}
