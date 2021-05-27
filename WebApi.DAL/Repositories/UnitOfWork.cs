using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApi.DAL.Interfaces;

namespace WebApi.DAL.Repositories
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private bool disposed;
        private readonly TContext context;
        private Dictionary<Type, object> repositories;

        public UnitOfWork(TContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            repositories = new Dictionary<Type, object>();
            disposed = false;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (repositories.Keys.Contains(typeof(TEntity)))
            {
                return repositories[typeof(TEntity)] as IRepository<TEntity>;
            }

            var repository = new Repository<TEntity>(context);
            repositories.Add(typeof(TEntity), repository);
            return repository;
        }  

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
