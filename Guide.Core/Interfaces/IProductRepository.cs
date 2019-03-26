using System.Collections.Generic;
using Guide.Common.Interfaces;
using Guide.Core.DomainModels;

namespace Guide.Core.Interfaces
{
    public interface IProductRepository : IRepository<Product, int>
    {
        IEnumerable<Product> GetAllProducts();
    }
}
