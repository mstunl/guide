using Guide.Common;

namespace Guide.Core.Interfaces
{

    public interface IRepository<TEntity, in TKey>
        where TEntity : EntityBase<TKey>
    {
        TEntity Get(TKey id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TKey id);
    }


    //public interface IRepository<TEntity, in TKey> : IReadRepository<TEntity, TKey>, IWriteRepository<TEntity, TKey>
    //    where TEntity : EntityBase<TKey>
    //{
    //}

    //public interface IReadRepository<TEntity, in TKey> where TEntity: class
    //{
    //    TEntity Get(TKey id);
    //    IQueryable<TEntity> GetAll();
    //    IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
    //}

    //public interface IWriteRepository<in TEntity, in TKey>
    //{
    //    void Insert(TEntity entity);
    //    void Update(TEntity entity);
    //    void Delete(TKey id);
    //}
}
