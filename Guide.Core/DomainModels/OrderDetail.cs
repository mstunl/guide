using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Guide.Common;

namespace Guide.Core.DomainModels
{
    public class OrderDetail : AuditableEntityBase<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }

    }
}
