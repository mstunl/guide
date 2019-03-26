using Guide.Common.Interfaces;
using Guide.Core.DomainModels;

namespace Guide.Core.Interfaces
{
    public interface IOrderRepository : IRepository<Order, int>
    {
    }
}
