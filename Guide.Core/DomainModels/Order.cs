using System;
using System.Collections.Generic;
using System.Text;
using Guide.Common;

namespace Guide.Core.DomainModels
{
    public class Order : AuditableEntityBase<int>
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int DealerId { get; set; }
        
    }
}
