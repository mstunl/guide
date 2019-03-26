using MediatR;
using System;
using System.Linq.Expressions;

namespace Guide.Application.CQRS
{
    public abstract class BaseRequest<TResult, TEntity> : IRequest<TResult>
    {
        protected Expression<Func<TEntity, bool>> Predicate { get; set; }
    }
}
