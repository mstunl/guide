namespace Guide.Core
{
    public abstract class DtoBase<TKey>
    {
        public TKey Id { get; protected set; }
    }
}
