using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guide.Application.MediatrDecorators.Logging
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class AuditLogAttribute : Attribute
    {
        public AuditLogAttribute()
        {
            
        }
    }
}
