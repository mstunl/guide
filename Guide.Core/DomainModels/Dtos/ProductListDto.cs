using System;
using System.Collections.Generic;
using System.Text;
using Guide.Common;
using Guide.Common.Interfaces;

namespace Guide.Core.DomainModels.Dtos
{
    public class ProductListDto : DtoBase<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int StockAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
