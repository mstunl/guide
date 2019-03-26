using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Guide.Application.ExceptionHandling
{
    public class ValidationException : Exception
    {
        public ValidationException()
        {
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public ValidationException(string message) 
            : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
