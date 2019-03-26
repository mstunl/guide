namespace Guide.Application.CQRS
{
    public interface IPreRequestHandler<in TRequest>
    {
        void Handle(TRequest request);
    }
}
