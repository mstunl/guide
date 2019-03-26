using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Guide.Application.CQRS
{
    public class MediatorPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
           where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly IEnumerable<IPreRequestHandler<TRequest>> _preRequestHandlers;
        private readonly IEnumerable<IPostRequestHandler<TRequest, TResponse>> _postRequestHandlers;
        public MediatorPipeline(
            IEnumerable<IValidator<TRequest>> validators,
            IRequestHandler<TRequest, TResponse> inner,
            IEnumerable<IPreRequestHandler<TRequest>> preRequestHandlers,
            IEnumerable<IPostRequestHandler<TRequest, TResponse>> postRequestHandlers)
        {
            _validators = validators;
            _inner = inner;
            _preRequestHandlers = preRequestHandlers;
            _postRequestHandlers = postRequestHandlers;
        }
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            foreach (var preRequestHandler in _preRequestHandlers)
                preRequestHandler.Handle(request);

            #region Validation
            var context = new FluentValidation.ValidationContext(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new FluentValidation.ValidationException(failures);
            }
            #endregion

            var response = _inner.Handle(request, cancellationToken);

            foreach (var postRequestHandler in _postRequestHandlers)
                postRequestHandler.Handle(request, response.Result);

            //return next();
            return response;
        }
    }
}
