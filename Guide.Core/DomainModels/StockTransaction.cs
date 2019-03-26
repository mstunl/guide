using System;
using System.Collections.Generic;
using System.Text;
using Guide.Common;

namespace Guide.Core.DomainModels
{
    public class StockTransaction : AuditableEntityBase<int>
    {
        public int DealerId { get; set; }
        public int ProductId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Quantity { get; set; }
    }
}
