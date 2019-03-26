using System;
using System.Collections.Generic;
using System.Text;
using Guide.Common;

namespace Guide.Core.DomainModels
{
    public class Dealer: AuditableEntityBase<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
