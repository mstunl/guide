using Guide.Common;
using Guide.Common.Interfaces;
using Guide.Core;
using Guide.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Data.Repositories
{
    public class GenericRepository<TEntity, TKey> : RepositoryBase<GuideContext>, IRepository<TEntity, TKey> 
        where TEntity: EntityBase<TKey>
    {
        protected DbSet<TEntity> DbSet;
        public GenericRepository(GuideContext context) : base(context)
        {
        }

        public TEntity Get(TKey id)
        {
            DbSet = Context.Set<TEntity>();
            return DbSet.Find(id);
        }
        
        public void Insert(TEntity entity)
        {
            //entity.ValidateForPersistence();
            DbSet = Context.Set<TEntity>();
            DbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            //entity.ValidateForPersistence();
            DbSet = Context.Set<TEntity>();
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TKey id)
        {
            DbSet = Context.Set<TEntity>();
            var entity = DbSet.Find(id);
            //entity.ValidateForDelete();
            DbSet.Remove(entity);
        }
    }

    //public class GenericReadRepository<TEntity,TKey> : RepositoryBase<GuideContext>, IReadRepository<TEntity, TKey> where TEntity: EntityBase<TKey>
    //{
    //    protected DbSet<TEntity> DbSet;
    //    public GenericReadRepository(GuideContext context) : base(context)
    //    {
    //    }
    //    public TEntity Get(TKey id)
    //    {
    //        DbSet = Context.Set<TEntity>();
    //        return DbSet.Find(id);
    //    }

    //    public IQueryable<TEntity> GetAll()
    //    {
    //        DbSet = Context.Set<TEntity>();
    //        return DbSet;
    //    }

    //    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
    //    {
    //        DbSet = Context.Set<TEntity>();
    //        return DbSet.Where(predicate);
    //    }
    //}

    //public class GenericWriteRepository<TEntity, TKey> : RepositoryBase<GuideContext>, IWriteRepository<TEntity, TKey>
    //    where TEntity : EntityBase<TKey>
    //{
    //    protected DbSet<TEntity> DbSet;
    //    public GenericWriteRepository(GuideContext context) : base(context)
    //    {
    //    }
        
    //    public void Insert(TEntity entity)
    //    {
    //        //entity.ValidateForPersistence();
    //        DbSet = Context.Set<TEntity>();
    //        DbSet.Add(entity);
    //    }

    //    public void Update(TEntity entity)
    //    {
    //        //entity.ValidateForPersistence();
    //        DbSet = Context.Set<TEntity>();
    //        Context.Entry(entity).State = EntityState.Modified;
    //    }

    //    public void Delete(TKey id)
    //    {
    //        DbSet = Context.Set<TEntity>();
    //        var entity = DbSet.Find(id);
    //        //entity.ValidateForDelete();
    //        DbSet.Remove(entity);
    //    }
    //}
}
