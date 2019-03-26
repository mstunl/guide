namespace Guide.Application.Data
{
    public abstract class RepositoryBase<TContext>
    {
        protected readonly TContext Context;

        protected RepositoryBase(TContext context)
        {
            Context = context;
        }
    }
}
