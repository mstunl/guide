using Guide.Application.Data.Repositories;
using Guide.Common;
using Guide.Common.Interfaces;
using Guide.Core.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Guide.Core;

namespace Guide.Application.Data.UnitOfWork
{
    public class UnitOfWork<TKey> : IUnitOfWork<TKey>
    {
        private readonly GuideContext _dbContext;
        private bool _disposed = false;
        private readonly IEventStorage<TKey> _eventStorage;
        
        public UnitOfWork(GuideContext dbContext, IRepository<EntityBase<int>, int> genericRepository, IEventStorage<TKey> eventStorage)
        {
            _dbContext = dbContext;
            _eventStorage = eventStorage;
        }


        #region Generic Repositories
        //public IRepository<EntityBase<TKey>, TKey> GetGenericRepository<TEntity>() where TEntity : class
        //{
        //    return new GenericRepository<EntityBase<TKey>, TKey>(_dbContext);
        //}

        public IRepository<EntityBase<TKey>, TKey> GenericRepo => new GenericRepository<EntityBase<TKey>, TKey>(_dbContext);
        public IProductRepository ProductRepository => new ProductRepository(_dbContext);

        #endregion

        #region Entity Repositories

        #endregion


        #region Save&Dispose



        public bool SaveChanges(EntityBase<TKey> entity)
        {
            try
            {
                // Transaction işlemleri burada ele alınabilir veya Identity Map kurumsal tasarım kalıbı kullanılarak
                // sadece değişen alanları güncellemeyi de sağlayabiliriz.

                _eventStorage.Save(entity);
                return _dbContext.SaveChanges() >= 0;
            }
            catch
            {
                // Burada DbEntityValidationException hatalarını handle edebiliriz.
                throw;
            }
        }
        public async Task<int> SaveChangesAsync(EntityBase<TKey> entity, CancellationToken cancellationToken)
        {
            try
            {
                _eventStorage.Save(entity);
                return await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }

        }


        public void Rollback()
        {
            _dbContext
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        } 
        #endregion





    }
}
