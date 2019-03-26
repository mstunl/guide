using Guide.Common;
using Guide.Common.Interfaces;
using Guide.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Guide.Core;

namespace Guide.Application.Data.UnitOfWork
{
    public interface IUnitOfWork<TKey> : IDisposable
    {
        IRepository<EntityBase<TKey>, TKey> GenericRepo { get; }
        IProductRepository ProductRepository { get; }

        bool SaveChanges(EntityBase<TKey> entity);
        Task<int> SaveChangesAsync(EntityBase<TKey> entity, CancellationToken cancellationToken);

        void Rollback();
    }
}
