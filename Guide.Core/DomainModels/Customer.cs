using System;
using System.Collections.Generic;
using System.Text;
using Guide.Common;

namespace Guide.Core.DomainModels
{
    public class Customer: AuditableEntityBase<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
