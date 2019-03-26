using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Guide.Application.MediatrDecorators.Logging
{
    public class AuditLoggingDecorator<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : IRequest<TResponse>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IRequestHandler<TCommand, TResponse> _handler;

        public AuditLoggingDecorator(ILoggerFactory loggerFactory, IRequestHandler<TCommand, TResponse> handler)
        {
            _loggerFactory = loggerFactory;
            _handler = handler;
        }


        public Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            var logger = _loggerFactory.CreateLogger<TCommand>();
            logger.LogInformation($"Audit Log Data: {requestJson}");

            return _handler.Handle(request, cancellationToken);

        }
    }
}
