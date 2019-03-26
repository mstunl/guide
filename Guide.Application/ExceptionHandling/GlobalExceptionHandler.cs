using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Guide.Application.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            string message;
            var ex = context.Exception;
            //var exceptionType = context.Exception.GetType();
            if (ex is UnauthorizedAccessException)
            {
                message = "Unauthorized Access";
                status = HttpStatusCode.Unauthorized;
            }
            else if (ex is NotImplementedException)
            {
                message = "A server error occurred.";
                status = HttpStatusCode.NotImplemented;
            }
            else if (ex is FluentValidation.ValidationException)
            {
                message = ex.Message;
                status = HttpStatusCode.BadRequest;
                _logger.LogError(message);
            }
            else if (ex is ValidationException)
            {
                message = ValidationExceptionManager.GetCustomValidationExceptionMessage(ex);
                status = HttpStatusCode.InternalServerError;
                _logger.LogError(message);
            }
            else
            {
                message = ex.Message;
                status = HttpStatusCode.NotFound;
            }
            context.ExceptionHandled = true;

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
            var err = message + " " + context.Exception.StackTrace;
            response.WriteAsync(err);
        }

    }
}
