using System;
using System.Collections.Generic;
using System.Text;
using Guide.Common.Interfaces;
using Guide.Core.DomainModels;

namespace Guide.Core.Interfaces
{
    public interface IStockTransactionRepository : IRepository<StockTransaction, int>
    {
    }
}
