using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Guide.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Guide.Application.MediatrDecorators.Logging
{
    public class RequestLogger<TRequest> : IPreRequestHandler<TRequest>
    {
        private readonly ILoggerFactory _loggerFactory;

        public RequestLogger(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }


        public void Handle(TRequest request)
        {
            var logger = _loggerFactory.CreateLogger<TRequest>();
            var requestType = request.GetType().Name;

            logger.LogInformation("RequestType: " + "_" + requestType + "_" + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
        }
    }
}
