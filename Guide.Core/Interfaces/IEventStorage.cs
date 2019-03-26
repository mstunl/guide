namespace Guide.Core.Interfaces
{
    public interface IEventStorage<TKey>
    {
        void Save(EntityBase<TKey> entity);
    }
}
